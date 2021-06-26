using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BackChannel.Classes;

namespace BackChannel.ViewModels
{
    /// <summary>
    /// Holds and handles updating of the list of channels on the middle panel
    /// </summary>
    public class ChannelViewModel : INotifyPropertyChanged
    {
        // UI notification functions
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // The collections of channels
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

        // Helper functions
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
