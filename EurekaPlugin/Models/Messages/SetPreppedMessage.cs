using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models.Messages
{
    class SetPreppedMessage
    {
        public int id;
        public bool prepped;

        public SetPreppedMessage(int id, bool prepped)
        {
            this.id = id;
            this.prepped = prepped;
        }
    }
}
