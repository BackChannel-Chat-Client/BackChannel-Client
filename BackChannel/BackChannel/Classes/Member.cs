using System.Windows.Media;

namespace BackChannel.Classes
{
    /// <summary>
    /// Represents a member in a channel.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// The name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Temp variable as it may change, but this will be the pfp of the member.
        /// </summary>
        public byte[] PFP { get; set; }

        /// <summary>
        /// The color to show as the status, green / yellow / red / gray.
        /// </summary>
        public SolidColorBrush OnlineStatus { get; set; }
    }
}
