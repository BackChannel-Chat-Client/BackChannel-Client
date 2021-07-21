using System.Collections.Generic;

namespace BackChannel.Classes
{
    /// <summary>
    /// Holds all the info about a joined server
    /// </summary>
    public class Server
    {
        /// <summary>
        /// The name of the server, this is defaulted to the ip/domain but can be set by the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All of the channels in the server viewable by the user.
        /// </summary>
        public List<Channel> Channels { get; set; }

        public int Port { get; set; }

        public bool AllowSelfSigned { get; set; }
    }
}
