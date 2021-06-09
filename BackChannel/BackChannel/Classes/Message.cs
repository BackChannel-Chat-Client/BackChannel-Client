using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public Visibility IsSolo { get; set; }
        public Thickness TextMargin { get; set; }
        public DateTime Date { get; set; }
        public Member User { get; set; }

        public Message()
        {
            IsSolo = Visibility.Visible;
            TextMargin = new Thickness(0, 0, 0, 0);
        }
    }
}
