using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChannel.Enums
{
    public enum MessageType
    {
        IsText = 0,
        IsFile = 1,
        IsImage = 2,
        IsVideo = 3,
        IsEmbed = 4,
    }
}
