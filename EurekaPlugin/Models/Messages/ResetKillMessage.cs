using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models.Messages
{
    class ResetKillMessage
    {
        public int id;

        public ResetKillMessage(int id)
        {
            this.id = id;
        }
    }
}
