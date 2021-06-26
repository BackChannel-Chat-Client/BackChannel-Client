namespace BackChannel.Enums
{
    /// <summary>
    /// The buttons to show on a debug popup.
    /// </summary>
    public enum DebugPopupType
    {
        /// <summary>
        /// Only shows the close popup button.
        /// </summary>
        Notify = 0,
        /// <summary>
        /// Shows the close popup and close backchannel buttons.
        /// </summary>
        PossiblyFatal = 1,
        /// <summary>
        /// Only shows the close backchannel button.
        /// </summary>
        Fatal = 2,
        /// <summary>
        /// Shows the Cancel and Allow buttons for Self signed sertificate errors.
        /// </summary>
        Security = 3
    }
}
