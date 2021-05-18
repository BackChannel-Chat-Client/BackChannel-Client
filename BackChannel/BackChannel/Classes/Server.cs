using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChannel.Classes
{
    /// <summary>
    /// Holds all the info about a joined server
    /// </summary>
    public class Server
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Channel> Channels { get; set; }
    }
}
