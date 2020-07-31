using Dalamud.Plugin;
using EurekaPlugin.Models;
using EurekaPlugin.Models.EurekaTracker;
using EurekaPlugin.Models.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EurekaPlugin
{
    class EurekaTrackerConnection : IDisposable
    {
        private const string TrackerUrl = "wss://ffxiv-eureka.com/socket/websocket?vsn=2.0.0";
        
        private readonly DalamudPluginInterface pluginInterfance;

        private string TrackerId;
        private bool isConnected;
        private ClientWebSocket Socket;
        private CancellationTokenSource CancellationTokenSource;
        private int LastHeartbeatId;
        private int NextMessageId;
        private int Viewers;
        private string Password;
        private IEurekaTracker Tracker;

        public EurekaTrackerConnection(string trackerId, DalamudPluginInterface plugin)
        {
            NextMessageId = 1;
            LastHeartbeatId = -1;
            this.TrackerId = trackerId;
            Viewers = -1;
            CancellationTokenSource = new CancellationTokenSource();
            Socket = new ClientWebSocket();
            this.pluginInterfance = plugin;
        }

        public async Task<bool> Connect()
        {
            try
            {
                await Socket.ConnectAsync(new Uri(TrackerUrl), CancellationTokenSource.Token);
                Receive();
                PluginLog.Log("Conncted to websocket");
                return true;
            }
            catch (Exception ex)
            {
                PluginLog.LogError(ex, "Unable to connect to websocket");
            }
            return false;
        }

        public async Task Send(string data)
        {
            await Socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationTokenSource.Token);
        }

        public async Task Receive()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
            do
            {
                WebSocketReceiveResult result;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    do
                    {
                        result = await Socket.ReceiveAsync(buffer, CancellationTokenSource.Token);
                        memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        string data = await reader.ReadToEndAsync();
                        JArray messageArray = JArray.Parse(data);
                        EurekaTrackerMessage message = new EurekaTrackerMessage (
                            messageArray[0].Type != JTokenType.Null,
                            messageArray[1].Type == JTokenType.Null ? -1 : (int)messageArray[1],
                            (string)messageArray[2],
                            (string)messageArray[3],
                            (JObject)messageArray[4]
                        );
                        switch (message.Event)
                        {
                            case "phx_reply": // Reply sent by server about the status of most outgoing requests. Notably used for heartbeats.
                                if (message.MessageId == LastHeartbeatId)
                                {
                                    LastHeartbeatId = -1;
                                    break;
                                }
                                if (message.Payload["response"].Type == JTokenType.String) 
                                {
                                    string responseMessage = (string)message.Payload["response"];
                                    if (responseMessage.Equals("Instance does not exist"))
                                    {
                                        TrackerId = "INVALID";
                                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationTokenSource.Token);
                                    }
                                }
                                break;
                            case "presence_state": // Initial list of indentifiers for people using the channel
                                Viewers = message.Payload.Count;
                                break;
                            case "presence_diff": // Someone has joined or left the tracker
                                JObject joins = (JObject)message.Payload["joins"];
                                JObject leaves = (JObject)message.Payload["leaves"];
                                Viewers = Viewers + joins.Count - leaves.Count;
                                break;
                            case "payload":
                                if (!message.Channel.Equals(GetInstanceChannel()))
                                    break;
                                {
                                    JObject attributesObject = (JObject)message.Payload["data"]["attributes"];
                                    if (attributesObject.ContainsKey("notorious-monsters"))
                                    {
                                        JObject monsterList = JObject.Parse((string)attributesObject["notorious-monsters"]);
                                        Dictionary<int, long> killTimes = new Dictionary<int, long>();
                                        foreach (JToken token in monsterList.Children())
                                        {
                                            JProperty property = (JProperty)token;
                                            killTimes.Add(int.Parse(property.Name), (long)property.Value);
                                        }
                                        Tracker.SetKilledTimes(killTimes);
                                    }
                                    if (attributesObject.ContainsKey("prepped-notorious-monsters"))
                                    {
                                        JObject preppedObject = JObject.Parse((string)attributesObject["prepped-notorious-monsters"]);
                                        Dictionary<int, bool> preppedList = new Dictionary<int, bool>();
                                        foreach (JToken token in preppedObject.Children())
                                        {
                                            JProperty property = (JProperty)token;
                                            preppedList.Add(int.Parse(property.Name), (bool)property.Value);
                                        }
                                        Tracker.SetPreppedList(preppedList);
                                    }
                                }
                                break;
                            case "initial_payload":
                                if (!message.Channel.Equals(GetInstanceChannel()))
                                    break;
                                isConnected = true;
                                int zoneId = (int)message.Payload["data"]["relationships"]["zone"]["data"]["id"];
                                switch(zoneId)
                                {
                                    case 1:
                                        Tracker = EurekaTrackerAnemos.GenerateNewAnemosTracker();
                                        break;
                                    case 2:
                                        Tracker = EurekaTrackerPagos.GenerateNewPagosTracker();
                                        break;
                                    case 3:
                                        Tracker = EurekaTrackerPyros.GenerateNewPyrosTracker();
                                        break;
                                    case 4:
                                    default:
                                        Tracker = EurekaTrackerHydatos.GenerateNewHydatosTracker();
                                        break;
                                }
                                {
                                    JObject attributesObject = (JObject)message.Payload["data"]["attributes"];
                                    Password = attributesObject.ContainsKey("password") ? (string)attributesObject["Password"] : null;
                                    JObject monsterList = JObject.Parse((string)attributesObject["notorious-monsters"]);
                                    Dictionary<int, long> killTimes = new Dictionary<int, long>();
                                    foreach (JToken token in monsterList.Children())
                                    {
                                        JProperty property = (JProperty)token;
                                        killTimes.Add(int.Parse(property.Name), (long)property.Value);
                                    }
                                    Tracker.SetKilledTimes(killTimes);
                                }
                                {
                                    JObject attributesObject = (JObject)message.Payload["data"]["attributes"];
                                    JObject preppedObject = JObject.Parse((string)attributesObject["prepped-notorious-monsters"]);
                                    Dictionary<int, bool> preppedList = new Dictionary<int, bool>();
                                    foreach (JToken token in preppedObject.Children())
                                    {
                                        JProperty property = (JProperty)token;
                                        preppedList.Add(int.Parse(property.Name), (bool)property.Value);
                                    }
                                    Tracker.SetPreppedList(preppedList);
                                }
                                break;
                            case "password_set":
                                bool wasSuccessful = (bool)message.Payload["success"];
                                if (wasSuccessful)
                                {
                                    Password = (string)message.Payload["password"];
                                } 
                                else
                                {
                                    Password = null;
                                }
                                break;
                        }
                    }
                }
            } while (!CancellationTokenSource.Token.IsCancellationRequested);
        }

        public async Task SetPrepped(int monsterId, bool prepped)
        {
            SetPreppedMessage preppedMessage = new SetPreppedMessage(monsterId, prepped);
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "set_prepped", JObject.Parse(JsonConvert.SerializeObject(preppedMessage)));
            NextMessageId++;
            await Send(message.ToMessage());
        }

        public async Task SetKilled(int monsterId, long killTime)
        {
            SetKillTimeMessage preppedMessage = new SetKillTimeMessage(monsterId, killTime);
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "set_kill_time", JObject.Parse(JsonConvert.SerializeObject(preppedMessage)));
            NextMessageId++;
            await Send(message.ToMessage());
        }

        public async Task SetPassword(string password)
        {
            SetPasswordMessage preppedMessage = new SetPasswordMessage(password);
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "set_password", JObject.Parse(JsonConvert.SerializeObject(preppedMessage)));
            NextMessageId++;
            await Send(message.ToMessage());
        }

        public async Task ResetKill(int monsterId)
        {
            ResetKillMessage preppedMessage = new ResetKillMessage(monsterId);
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "reset_kill", JObject.Parse(JsonConvert.SerializeObject(preppedMessage)));
            NextMessageId++;
            await Send(message.ToMessage());
        }

        public async Task ResetAll()
        {
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "reset_all", null);
            NextMessageId++;
            await Send(message.ToMessage());
        }

        public async Task Join()
        {
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "phx_join", new JObject());
            NextMessageId++;
            await Send(message.ToMessage());
            Task.Run(async () =>
            {
                while (!CancellationTokenSource.Token.IsCancellationRequested)
                {
                    Heartbeat();
                    await Task.Delay(TimeSpan.FromSeconds(30), CancellationTokenSource.Token);
                }
            }, CancellationTokenSource.Token);
        }

        public async Task Join(string password)
        {
            SetPasswordMessage preppedMessage = new SetPasswordMessage(password);
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "phx_join", JObject.Parse(JsonConvert.SerializeObject(preppedMessage)));
            NextMessageId++;
            await Send(message.ToMessage());
            Task.Run(async () =>
            {
                while (!CancellationTokenSource.Token.IsCancellationRequested)
                {
                    Heartbeat();
                    await Task.Delay(TimeSpan.FromSeconds(30), CancellationTokenSource.Token);
                }
            }, CancellationTokenSource.Token);
        }

        public async Task Leave()
        {
            isConnected = false;
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, GetInstanceChannel(), "phx_leave", new JObject());
            NextMessageId++;
            await Send(message.ToMessage());
            CancellationTokenSource.Cancel();
            await Task.Delay(TimeSpan.FromSeconds(3), CancellationToken.None);
            await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
        }

        public async Task Heartbeat()
        {
            if (LastHeartbeatId != -1)
            {
                PluginLog.LogError("No response to heartbeat message recieved within 30 seconds. Disconnecting...");
                await Leave();
                Dispose();
                return;
            }
            EurekaTrackerMessage message = new EurekaTrackerMessage(true, NextMessageId, "phoenix", "heartbeat", new JObject());
            LastHeartbeatId = NextMessageId;
            NextMessageId++;
            await Send(message.ToMessage());
        }

        public bool CanModify()
        {
            return Password != null;
        }

        public string GetPassword()
        {
            return Password;
        }

        public bool IsConnected()
        {
            return isConnected || TrackerId.Equals("INVALID");
        }

        public string GetTrackerId()
        {
            return TrackerId;
        }

        public int GetViewerCount()
        {
            return Viewers;
        }

        public IEurekaTracker GetTracker()
        {
            return Tracker;
        }

        public string GetInstanceChannel()
        {
            return "instance:" + TrackerId;
        }

        public void Dispose()
        {
            Leave();
        }
    }
}
