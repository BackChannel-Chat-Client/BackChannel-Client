namespace BackChannel.Enums
{
    /// <summary>
    /// The response status returned by the server.
    /// </summary>
    public enum ResponseStatus : uint
    {
        /// <summary>
        /// BCAPI call was successful.
        /// </summary>
        Success = 0x00000000,
        /// <summary>
        /// Server had an issue with this request.
        /// </summary>
        RequestError = 0xFFFFFFFF,
        /// <summary>
        /// The packet sent was invalid.
        /// </summary>
        InvalidPacket = 0x40000000,
        /// <summary>
        /// This request is unimplemented.
        /// </summary>
        UnImplemented = 0x40000001,
        /// <summary>
        /// There is more data comming, keep receiving 
        /// </summary>
        MoreData = 0x30000001
    }
}
