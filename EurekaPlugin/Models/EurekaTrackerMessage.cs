using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models.EurekaTracker
{
    class EurekaTrackerMessage
    {
        public EurekaTrackerMessage(bool setFirstValue, int messageId, string channel, string @event, JObject payload)
        {
            SetFirstValue = setFirstValue;
            MessageId = messageId;
            Channel = channel;
            Event = @event;
            Payload = payload;
        }

        public bool SetFirstValue { get; set; }
        public int MessageId { get; set; }
        public string Channel { get; set; }
        public string Event { get; set; }
        public JObject Payload { get; set; }

        public string ToMessage()
        {
            JArray array = new JArray();
            array.Add(SetFirstValue ? "1" : null);
            array.Add(MessageId.ToString());
            array.Add(Channel);
            array.Add(Event);
            if (Payload == null)
            {
                array.Add("null");
            }
            else
            {
                array.Add(Payload);
            }
            return array.ToString();
        }
    }
}
