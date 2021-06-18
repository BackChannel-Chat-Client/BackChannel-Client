using BackChannel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BackChannel.Classes
{
    /// <summary>
    /// Holds all the info about a message
    /// </summary>
    public class Message
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public Member User { get; set; }
        public MessageType Type { get; set; }
        public Visibility IsSolo { get; set; }
        public Thickness TextMargin { get; set; }
    }
}
