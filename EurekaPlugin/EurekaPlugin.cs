﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.Chat;
using Dalamud.Game.Chat.SeStringHandling;
using Dalamud.Game.Chat.SeStringHandling.Payloads;
using Dalamud.Game.Command;
using Dalamud.Plugin;

namespace EurekaPlugin
{
    class EurekaPlugin : IDalamudPlugin
    {
        public string Name => "Eureka Plugin";

        private EurekaTrackerConnection connection;
        private DalamudPluginInterface pluginInterface;
        private EurekaTrackerUI ui;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.ui = new EurekaTrackerUI(pluginInterface, this);
            this.pluginInterface = pluginInterface;
            this.pluginInterface.UiBuilder.OnBuildUi += DrawUI;
            this.pluginInterface.Framework.Gui.Chat.OnChatMessage += Chat_OnChatMessage;
            connection = null; 
            this.pluginInterface.CommandManager.AddHandler("/peureka", new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });
        }

        private void Chat_OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            List<Payload> payloads = message.Payloads;
            foreach (Payload payload in payloads)
            {
                if (payload.Type == PayloadType.MapLink)
                {
                    MapLinkPayload mapLinkPayload = (MapLinkPayload)payload;
                    PluginLog.Log(payload.ToString());
                }
            }
        }

        private void OnCommand(string command, string arguments)
        {
            this.ui.Visible = true;
        }

        private void DrawUI()
        {
            this.ui.Draw();
        }

        public async void setConnection(string trackerId)
        {
            if (connection != null)
            {
                connection.Dispose();
            }
            connection = new EurekaTrackerConnection(trackerId, pluginInterface);
            await connection.Connect();
            await connection.Join();
        }

        public EurekaTrackerConnection getConnection()
        {
            return connection;
        }

        public void Dispose()
        {
            this.pluginInterface.UiBuilder.OnBuildUi -= DrawUI;
            this.pluginInterface.Framework.Gui.Chat.OnChatMessage -= Chat_OnChatMessage;
            if (connection != null)
            {
                connection.Dispose();
            }
        }
    }
}
