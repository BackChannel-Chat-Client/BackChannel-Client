using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackChannel.Classes
{
    /// <summary>
    /// Holds all the info about a channel
    /// </summary>
    public class Channel
    {
        public int ID { get; set; }
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
            ID = rnd.Next(100, 200000);
            string[] words = { "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer" };
            for (int i = 0; i < numOfChannels; i++)
            {
                Message m = new Message { Username = words[rnd.Next(0, words.Length)], Content = CreateRandomMessage(30) };
                try
                {
                    if (m.Username == Messages[Messages.Count() - 1].Username)
                    {
                        m.IsSolo = Visibility.Collapsed;
                        m.TextMargin = new Thickness(60, 0, 50, 0);
                    }
                }
                catch (Exception) { }
                Messages.Add(m);
            }
        }
    }
}
