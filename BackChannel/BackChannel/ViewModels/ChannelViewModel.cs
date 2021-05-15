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
    /// Holds all the info about a channel
    /// </summary>
    public class Channel
    {
        public string Name { get; set; }
        public Visibility IsText { get; set; }
        public Visibility IsVoice { get; set; }
        public List<Message> Messages { get; set; }

        public static string CreateRandomMessage(int numofwrds)
        {
            Random rnd = new Random();
            //Dictionary of strings
            string[] words = {"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            int numberOfWords = rnd.Next(1, numofwrds);

            string randomString = words[rnd.Next(0, words.Length)];
            for (int i = 0; i < numberOfWords; i++)
            {
                //Create combination of word + number
                randomString += $" {words[rnd.Next(0, words.Length)]}";
            }

            return randomString;

        }

        public Channel()
        {
            IsText = Visibility.Collapsed;
            IsVoice = Visibility.Collapsed;

            Random rnd = new Random();
            Messages = new List<Message>();
            int numOfChannels = rnd.Next(5, 20);
            for (int i = 0; i < numOfChannels; i++)
            {
                Messages.Add(new Message { Username = CreateRandomMessage(2), Content = CreateRandomMessage(30) });
            }
        }
    }

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
