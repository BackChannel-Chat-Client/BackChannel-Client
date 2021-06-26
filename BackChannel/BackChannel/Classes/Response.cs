using System;

namespace BackChannel.Classes
{
    /// <summary>
    /// Represents the info received in from a BCAPI call.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// The size of the response packet.
        /// </summary>
        public UInt32 PacketSize { get; set; }

        /// <summary>
        /// The ID of the packet, should match the sent ID.
        /// </summary>
        public UInt32 PacketID { get; set; }

        /// <summary>
        /// The status of the response, check against ResponseStatus enum.
        /// </summary>
        public UInt32 ResponseStatus { get; set; }

        /// <summary>
        /// The body of the response.
        /// </summary>
        public byte[] ResponseBody { get; set; }
    }
}
