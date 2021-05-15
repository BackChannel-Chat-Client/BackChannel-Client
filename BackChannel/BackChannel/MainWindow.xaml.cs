using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BackChannel.ViewModels;

namespace BackChannel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This holds the list of servers that the user has joined
        /// </summary>
        public ServerViewModel serverViewModel;
        public ChannelViewModel channelViewModel;
        public ChatViewModel chatViewModel;

        /// <summary>
        /// Holds the window that gets opened to display debug info and options
        /// </summary>
        DebugPanel debugPanel;

        public MainWindow()
        {
            InitializeComponent();
            serverViewModel = new ServerViewModel(); // Instantiates the ViewModel
            channelViewModel = new ChannelViewModel(); // Instantiates the ViewModel
            chatViewModel = new ChatViewModel(); // Instantiates the ViewModel
            ServerListView.ItemsSource = serverViewModel.Servers; // Set the Server List's Source to the ViewModel
            ChannelListView.ItemsSource = channelViewModel.Channels; // Set the Channel List's Source to the ViewModel
            ChatListView.ItemsSource = chatViewModel.Messages; // Set the Channel List's Source to the ViewModel
        }

        /// <summary>
        /// Opens the Settings Panel.
        /// 
        /// </summary>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (debugPanel == null)
            {
                debugPanel = new DebugPanel();
                debugPanel.Show();
            }
        }

        /// 
        /// These Functions are used to keep the left and middle panel from getting to long. 
        /// Min can be set in xaml, but max has to be set based on the current size of both.
        /// 
        private void LeftColumnStack_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
                Sets the left and middle column max, so that they dont go offscreen / cause the text pane to get to small
            */
            try
            {
                LeftGridDef.MaxWidth = MainGrid.MaxWidth - (MiddleGridDef.Width.Value) - 800;
                MiddleGridDef.MaxWidth = MainGrid.MaxWidth - (LeftGridDef.Width.Value) - 800;
                ServerTitle.Width += (MiddleGridDef.Width.Value - ServerTitle.Width) - 50;
                debugPanel.UpdateText(LeftGridDef.Width.ToString(), MiddleGridDef.Width.ToString(), RightGridDef.Width.ToString(), LeftGridDef.MaxWidth.ToString(), MiddleGridDef.MaxWidth.ToString());
            }
            catch (Exception)
            {

            }
        }
        private void MiddleColumnStack_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
                Sets the left and middle column max, so that they dont go offscreen / cause the text pane to get to small
            */
            try
            {
                LeftGridDef.MaxWidth = MainGrid.MaxWidth - (MiddleGridDef.Width.Value) - 800;
                MiddleGridDef.MaxWidth = MainGrid.MaxWidth - (LeftGridDef.Width.Value) - 800;
                ServerTitle.Width += (MiddleGridDef.Width.Value - ServerTitle.Width) - 50;
                debugPanel.UpdateText(LeftGridDef.Width.ToString(), MiddleGridDef.Width.ToString(), RightGridDef.Width.ToString(), LeftGridDef.MaxWidth.ToString(), MiddleGridDef.MaxWidth.ToString());
            }
            catch (Exception)
            {

            }
        }
        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
               Sets the left and middle column max, so that they dont go offscreen / cause the text pane to get to small
           */
            try
            {
                LeftGridDef.MaxWidth = MainGrid.MaxWidth - (MiddleGridDef.Width.Value) - 800;
                MiddleGridDef.MaxWidth = MainGrid.MaxWidth - (LeftGridDef.Width.Value) - 800;
                ServerTitle.Width += (MiddleGridDef.Width.Value - ServerTitle.Width) - 50;
                debugPanel.UpdateText(LeftGridDef.Width.ToString(), MiddleGridDef.Width.ToString(), RightGridDef.Width.ToString(), LeftGridDef.MaxWidth.ToString(), MiddleGridDef.MaxWidth.ToString());
            }
            catch (Exception)
            {

            }
        }

        
        private void AddServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Random rnd = new Random();
                //var mainwnd = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
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
                serverViewModel.AddServer(s);
            }
            catch (Exception)
            {

            }
        }

        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ServerListView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
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
        private async void ServerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = (Server)ServerListView.SelectedItem;
                ServerTitle.Text = item.Name;
                MiddleColumnStack.Visibility = Visibility.Visible;
                channelViewModel.Channels.Clear();
                await Task.Run(() =>
                {
                    foreach(Channel c in item.Channels)
                    {
                        channelViewModel.AddChannel(c);
                    }
                });
            }
            catch (Exception)
            {
                MiddleColumnStack.Visibility = Visibility.Collapsed;
            }
            RightColumnStack.Visibility = Visibility.Collapsed;
        }

        private async void ChannelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = (Channel)ChannelListView.SelectedItem;
                if (item.IsText == Visibility.Visible)
                {
                    ChannelTitle.Text = item.Name;
                    RightColumnStack.Visibility = Visibility.Visible;
                    chatViewModel.Messages.Clear();
                    await Task.Run(() =>
                    {
                        foreach (Message m in item.Messages)
                        {
                            chatViewModel.AddText(m);
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                ChatListView.ScrollIntoView(m);
                            }));
                        }

                    });
                }
                else
                {
                    ChannelListView.SelectedItem = null;
                }
            }
            catch (Exception)
            {
                //TextChannelGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void ServerSettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
