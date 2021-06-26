using System.Collections.Generic;
using System.Windows;

namespace BackChannel.Classes
{
    /// <summary>
    /// Holds all the info about a channel
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// The Channel ID.<br></br>
        /// Used for BCAPI calls
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Max number of messages a channel allows.<br></br>
        /// May be removed in the future as it may not need to be sent to the client.
        /// </summary>
        public int MaxMessages { get; set; }

        /// <summary>
        /// The name of the channel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IsText and IsVoice are set based on what type the channel is.<br></br>
        /// This may be changed in the future
        /// </summary>
        public Visibility IsText { get; set; }

        /// <summary>
        /// IsVoice and IsText are set based on what type the channel is.<br></br>
        /// This may be changed in the future
        /// </summary>
        public Visibility IsVoice { get; set; }

        /// <summary>
        /// This is the list of loaded messages
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// This is the list of members that can view the current channel
        /// </summary>
        public List<Member> Members { get; set; }
    }
}
