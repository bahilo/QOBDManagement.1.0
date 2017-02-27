using QOBDCommon.Interfaces.DAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Entities
{
    public class Safe
    {
        public Agent AuthenticatedUser { get; set; }
        public bool IsAuthenticated { get; set; }

        public Safe()
        {
            AuthenticatedUser = new Agent();
        }    
    }
}
