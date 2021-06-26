using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BackChannel.Classes;

namespace BackChannel.ViewModels
{
    /// <summary>
    /// Holds and handles updating of the list of joined servers on the left panel
    /// </summary>
    public class ServerViewModel : INotifyPropertyChanged
    {
        // UI Notifications functions
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // The collection of servers
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

        // Helper functions
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
