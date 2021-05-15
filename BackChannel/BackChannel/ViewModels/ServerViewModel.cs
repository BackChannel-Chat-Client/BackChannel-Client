using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BackChannel.ViewModels
{
    /// <summary>
    /// Holds all the info about a joined server
    /// </summary>
    public class Server
    {
        public string Name { get; set; }
        public List<Channel> Channels { get; set; }
    }

    /// <summary>
    /// Holds and handles updating of the list of joined servers on the left panel
    /// </summary>
    public class ServerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Server> servers = new ObservableCollection<Server>();
        public ObservableCollection<Server> Servers
        {
            get { return servers; }
            set
            {
                servers = value;
                this.OnPropertyChanged("Servers");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void AddServer(Server s)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Servers.Add(s);
            }));
        }

        public void RemoveServer(Server s)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Servers.Remove(s);
            }));
        }
    }
}
