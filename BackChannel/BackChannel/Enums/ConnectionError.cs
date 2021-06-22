using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChannel.Enums
{
    public enum ConnectionError
    {
        SelfSigned = 1,
        CertError = 2,
        ConnectionRefused = 3
    }
}
