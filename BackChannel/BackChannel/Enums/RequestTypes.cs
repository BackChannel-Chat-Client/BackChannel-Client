using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChannel.Enums
{
    public enum RequestType : uint
    {
        /// <summary>
        /// Get the last error
        /// </summary>
        GetLastError = 0x00,
        /// <summary>
        /// Verify your authentication key
        /// </summary>
        VerifyAuth = 0x01,
        /// <summary>
        /// Register for a new authentication key
        /// </summary>
        RegisterNewAuth = 0x02,
        /// <summary>
        /// Verify the integrity of communications
        /// </summary>
        VerifySecurity = 0x03,
        /// <summary>
        /// Get a list of all channel ids accessible by your auth key
        /// </summary>
        GetChannelList = 0x10,
        /// <summary>
        /// Send a message to a channel by ID
        /// </summary>
        SendMessage = 0x11,
        /// <summary>
        /// Get a message by its ID
        /// </summary>
        GetMessage = 0x12,
    }
}
