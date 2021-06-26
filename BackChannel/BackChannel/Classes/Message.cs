using BackChannel.Enums;
using System;
using System.Windows;

namespace BackChannel.Classes
{
    /// <summary>
    /// Holds all the info about a message in a channel.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The message ID, used an BCAPI calls.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The username of the message's creator.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The text of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The date/time when the message was sent.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The member that the message is tied to.<br>
        /// This is subject to change.</br>
        /// </summary>
        public Member User { get; set; }

        /// <summary>
        /// The type of the message.<br></br>
        /// Indicates if the message contains a file, image, text, etc.
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Visible if the message is not below another message sent by the same user.
        /// </summary>
        public Visibility IsSolo { get; set; }

        /// <summary>
        /// The margin of the text, used to make the margin smaller when it's below a message sent by the same user.
        /// </summary>
        public Thickness TextMargin { get; set; }
    }
}
