namespace BackChannel.Enums
{
    /// <summary>
    /// The error returned when trying to send a packet.
    /// </summary>
    public enum ConnectionError
    {
        /// <summary>
        /// There was no issue.
        /// </summary>
        None = 0,
        /// <summary>
        /// The certificate valid but is self signed, this makes it less secure and needs authorization to continue.
        /// </summary>
        SelfSigned = 1,
        /// <summary>
        /// The certificate is invalid.
        /// </summary>
        CertError = 2,
        /// <summary>
        /// The connection was refused by the server.
        /// </summary>
        ConnectionRefused = 3
    }
}
