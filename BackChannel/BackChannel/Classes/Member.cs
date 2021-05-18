using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BackChannel.Classes
{
    public class Member
    {
        public string Name { get; set; }
        public byte[] PFP { get; set; }
        public SolidColorBrush OnlineStatus { get; set; }
    }
}
