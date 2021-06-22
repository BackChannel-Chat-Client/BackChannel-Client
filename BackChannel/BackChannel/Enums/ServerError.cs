using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChannel.Enums
{
    public enum ResponseStatus : uint
    {
        Success = 0x00000000,
        RequestError = 0xFFFFFFFF,
        InvalidPacket = 0x40000000,
        UnImplemented = 0x40000001,
        MoreData = 0x30000001

    }
}
