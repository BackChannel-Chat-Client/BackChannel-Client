using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChannel.Enums
{
    public enum ServerError : uint
    {
        NoError = 0,
        RequestError =  0xFFFFFFFF,
        InvalidPacket = 0x40000000,
        UnImplemented = 0x40000001
    }
}
