using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models.Messages
{
    class SetPasswordMessage
    {
        public string password;

        public SetPasswordMessage(string password)
        {
            this.password = password;
        }
    }
}
