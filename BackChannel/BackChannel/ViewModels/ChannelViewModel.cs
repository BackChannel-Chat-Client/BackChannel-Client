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
using BackChannel.Classes;

namespace BackChannel.ViewModels
{
    

    /// <summary>
    /// Holds and handles updating of the list of channels on the middle panel
    /// </summary>
    public class ChannelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Channel> channels = new ObservableCollection<Channel>();
        public ObservableCollection<Channel> Channels
        {
            get { return channels; }
            set
            {
                channels = value;
                this.OnPropertyChanged("Channels");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void AddChannel(Channel c)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Channels.Add(c);
            }));
        }

        public void RemoveChannel(Channel c)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Channels.Remove(c);
            }));
        }
    }
}
