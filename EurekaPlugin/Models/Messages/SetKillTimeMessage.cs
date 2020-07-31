using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models.Messages
{
    class SetKillTimeMessage
    {
        public int id;
        public long time;

        public SetKillTimeMessage(int id, long time)
        {
            this.id = id;
            this.time = time;
        }
    }
}
