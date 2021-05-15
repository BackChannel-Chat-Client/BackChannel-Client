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
    /// Holds all the info about a message
    /// </summary>
    public class Message
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }

    /// <summary>
    /// Holds and handles updating of the list of messages in the chat panel on the right
    /// </summary>
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                this.OnPropertyChanged("Messages");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void AddText(Message t)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Messages.Add(t);
            }));
        }

        public void RemoveText(Message t)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Messages.Remove(t);
            }));
        }
    }
}
