using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BackChannel.Classes;

namespace BackChannel
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DebugPanel : Window
    {
        public DebugPanel()
        {
            InitializeComponent();
        }

        public static string CreateRandomName()
        {
            Random rnd = new Random();
            //Dictionary of strings
            string[] words = {"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            int numberOfWords = rnd.Next(1, 5);

            string randomString = words[rnd.Next(0, words.Length)];
            for (int i = 0; i < numberOfWords; i++)
            {
                //Create combination of word + number
                randomString += $" {words[rnd.Next(0, words.Length)]}";
            }

            return randomString;

        }

        public void UpdateText(string lw, string mw, string rw, string lm, string mm)
        {
            LeftWidthText.Text = $"Left Width: {lw}";
            MiddleWidthText.Text = $"Middle Width: {mw}";
            RightWidthText.Text = $"Right Width: {rw}";
            LeftMaxText.Text = $"Left Max: {lm}";
            MiddleMaxText.Text = $"Middle Max: {mm}";
        }

        public void UpdateMousePos(string pos)
        {
            MousePosText.Text = pos;
        }

        private void AddServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Random rnd = new Random();
                var mainwnd = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                Server s = new Server();
                s.Name = CreateRandomName();
                s.Channels = new List<Channel>();
                int numOfChannels = rnd.Next(5, 40);
                for (int i = 0; i < numOfChannels; i++)
                {
                    if (rnd.Next(0, 2) == 1)
                    {
                        s.Channels.Add(new Channel { Name = CreateRandomName(), IsVoice = Visibility.Visible });
                    }
                    else
                    {
                        s.Channels.Add(new Channel { Name = CreateRandomName(), IsText = Visibility.Visible });
                    }
                }
                mainwnd.serverViewModel.AddServer(s);
            }
            catch (Exception)
            {

            }
        }

        private void RemoveServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainwnd = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainwnd.serverViewModel.RemoveServer(mainwnd.serverViewModel.Servers[mainwnd.serverViewModel.Servers.Count() - 1]);
            }
            catch (Exception)
            {

            }
        }

        private void AddChannelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainwnd = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                Random rnd = new Random();
                if(rnd.Next(0,2) == 1)
                {
                    mainwnd.channelViewModel.AddChannel(new Channel { Name = CreateRandomName(), IsVoice = Visibility.Visible });
                }
                else
                {
                    mainwnd.channelViewModel.AddChannel(new Channel { Name = CreateRandomName(), IsText = Visibility.Visible });
                }
            }
            catch (Exception)
            {

            }
        }

        private void RemoveChannelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainwnd = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainwnd.channelViewModel.RemoveChannel(mainwnd.channelViewModel.Channels[mainwnd.channelViewModel.Channels.Count() - 1]);
            }
            catch (Exception)
            {

            }
        }

        public void AddTextToConsole(string text)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DebugConsole.AppendText(text + "\n");
                DebugConsole.Focus();
                DebugConsole.CaretIndex = DebugConsole.Text.Length;
                DebugConsole.ScrollToEnd();
            }));
        }

        private void TestRQButton_Click(object sender, RoutedEventArgs e)
        {
            PacketTester tester = new PacketTester();
            tester.Show();
            //Packet TestPacket = new Packet();
            //TestPacket.PacketID = 11111;
            //TestPacket.RequestType = 2;
            //TestPacket.RequestBody = Encoding.ASCII.GetBytes("\x0");
            //TestPacket.ChannelID = 001100;
            //TestPacket.AuthKey = Encoding.ASCII.GetBytes("TestAuth"); ;
            //TestPacket.GetPacketSize();
            //TestPacket.SendPacket();
            //
            //Response res = TestPacket.RecvResponse();
            //
            //AddTextToConsole($"[+] New Server Response:");
            //AddTextToConsole($"[Size] {res.PacketSize}\n[ID] {res.PacketID}\n[Status] {res.ResponseStatus}\n[Body] {res.ResponseBody}");
        }
    }
}
