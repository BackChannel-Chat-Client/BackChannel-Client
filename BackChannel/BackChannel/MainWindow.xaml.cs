using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BackChannel.ViewModels;
using BackChannel.Classes;
using System.Threading;

namespace BackChannel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Page variables
        public ServerViewModel serverViewModel; // List of servers you've joined
        public ChannelViewModel channelViewModel; // List of channels in a server
        public ChatViewModel chatViewModel; // Messages in a channel
        public MemberViewModel memberViewModel; // Members in a server
        public Thread LowerOpacityThread; // Thread for lowering settings opacity when changing font size
        public Thread RaiseOpacityThread;// Thread for raising settings opacity on mouse leave

        // A reference to the debug panel when open for debug logging 
        DebugPanel debugPanel;

        public MainWindow()
        {
            InitializeComponent();
            serverViewModel = new ServerViewModel(); // Instantiates the ViewModel
            channelViewModel = new ChannelViewModel(); // Instantiates the ViewModel
            chatViewModel = new ChatViewModel(); // Instantiates the ViewModel
            memberViewModel = new MemberViewModel(); // Instantiates the ViewModel
            ServerListView.ItemsSource = serverViewModel.Servers; // Set the Server List's Source to the ViewModel
            ChannelListView.ItemsSource = channelViewModel.Channels; // Set the Channel List's Source to the ViewModel
            ChatListView.ItemsSource = chatViewModel.Messages; // Set the Cannel List's Source to the ViewModel
            MemberListView.ItemsSource = memberViewModel.Members; // Set the Cannel List's Source to the ViewModel
            Debug.CreateSessionID();
        }

        // Column resizing handlers
        private void LeftColumnStack_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
                Sets the left and middle column max, so that they dont go offscreen / cause the text pane to get to small
            */
            try
            {
                LeftGridDef.MaxWidth = MainGrid.MaxWidth - (MiddleGridDef.Width.Value) - 800;
                MiddleGridDef.MaxWidth = MainGrid.MaxWidth - (LeftGridDef.Width.Value) - 800;
                ServerTitle.Width += (MiddleGridDef.Width.Value - ServerTitle.Width) - 20;
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
                ServerTitle.Width += (MiddleGridDef.Width.Value - ServerTitle.Width) - 20;
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
                ServerTitle.Width += (MiddleGridDef.Width.Value - ServerTitle.Width) - 20;
                debugPanel.UpdateText(LeftGridDef.Width.ToString(), MiddleGridDef.Width.ToString(), RightGridDef.Width.ToString(), LeftGridDef.MaxWidth.ToString(), MiddleGridDef.MaxWidth.ToString());
            }
            catch (Exception)
            {

            }
        }

        // Column scrollbar enable/disable functions
        private void ChannelListView_MouseEnter(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(ChannelListView, 0);

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            ChannelListView.Margin = new Thickness(10, 65, 0, 70);
        }
        private void ChannelListView_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(ChannelListView, 0);

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            ChannelListView.Margin = new Thickness(10, 65, 18, 70);
        }
        private void ServerListView_MouseEnter(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(ServerListView, 0);

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            ServerListView.Margin = new Thickness(0, 88, 0, 0);
        }
        private void ServerListView_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(ServerListView, 0);

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            ServerListView.Margin = new Thickness(0, 88, 18, 0);
        }
        private void ChatListView_MouseEnter(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(ChatListView, 0);

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            ChatListView.Margin = new Thickness(10, 65, 0, 40);

        }
        private void ChatListView_MouseLeave(object sender, MouseEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(ChatListView, 0);

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            ChatListView.Margin = new Thickness(10, 65, 18, 40);
        }

        // Server column functions
        private async void AddServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
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
                });
            }
            catch (Exception)
            {

            }
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

        // Channel column functions
        private async void ChannelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = (Channel)ChannelListView.SelectedItem;
                if (item.IsText == Visibility.Visible)
                {
                    ChannelTitle.Text = item.ID.ToString();
                    RightColumnStack.Visibility = Visibility.Visible;
                    chatViewModel.Messages.Clear();
                    memberViewModel.Members.Clear();
                    await Task.Run(() =>
                    {
                        foreach (Message m in item.Messages)
                        {
                            chatViewModel.AddText(m);
                        }
                        foreach (Member m in item.Members)
                        {
                            memberViewModel.AddMember(m);
                        }
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ChatListView.ScrollIntoView(chatViewModel.Messages[chatViewModel.Messages.Count() - 1]);
                        }));
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

        // Settings menu functions
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //if (debugPanel == null)
            //{
            //    debugPanel = new DebugPanel();
            //    debugPanel.Show();
            //}
            SettingsGrid.Visibility = Visibility.Visible;
        }
        private void CloseSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsGrid.Visibility = Visibility.Collapsed;
        }
        private void SettingsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)SettingsListView.SelectedItem;

            switch (item.Content)
            {
                case "Profile":
                    ProfileStack.Visibility = Visibility.Visible;
                    AppearanceStack.Visibility = Visibility.Collapsed;
                    NotificationsStack.Visibility = Visibility.Collapsed;
                    PrivacyStack.Visibility = Visibility.Collapsed;
                    VoiceVideoStack.Visibility = Visibility.Collapsed;
                    break;
                case "Appearance":
                    ProfileStack.Visibility = Visibility.Collapsed;
                    AppearanceStack.Visibility = Visibility.Visible;
                    NotificationsStack.Visibility = Visibility.Collapsed;
                    PrivacyStack.Visibility = Visibility.Collapsed;
                    VoiceVideoStack.Visibility = Visibility.Collapsed;
                    break;
                case "Notifications":
                    ProfileStack.Visibility = Visibility.Collapsed;
                    AppearanceStack.Visibility = Visibility.Collapsed;
                    NotificationsStack.Visibility = Visibility.Visible;
                    PrivacyStack.Visibility = Visibility.Collapsed;
                    VoiceVideoStack.Visibility = Visibility.Collapsed;
                    break;
                case "Privacy":
                    ProfileStack.Visibility = Visibility.Collapsed;
                    AppearanceStack.Visibility = Visibility.Collapsed;
                    NotificationsStack.Visibility = Visibility.Collapsed;
                    PrivacyStack.Visibility = Visibility.Visible;
                    VoiceVideoStack.Visibility = Visibility.Collapsed;
                    break;
                case "Voice/Video":
                    ProfileStack.Visibility = Visibility.Collapsed;
                    AppearanceStack.Visibility = Visibility.Collapsed;
                    NotificationsStack.Visibility = Visibility.Collapsed;
                    PrivacyStack.Visibility = Visibility.Collapsed;
                    VoiceVideoStack.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void LeftPanelClrPcker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

        }
        private void LowerOpacity()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                AppearanceTitle.Opacity = 0;
                ThemeStack.Opacity = 0;
            }));
            for (int i = 255; i > 149; i--)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    AppearanceStack.Background = new SolidColorBrush(Color.FromArgb((byte)i, 0, 0, 0));
                }));
                Thread.Sleep(1);
            }
        }
        private void RaiseOpacity()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                AppearanceTitle.Opacity = 1;
                ThemeStack.Opacity = 1;
            }));
            for (int i = 150; i < 256; i++)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    AppearanceStack.Background = new SolidColorBrush(Color.FromArgb((byte)i, 0, 0, 0));
                }));
                Thread.Sleep(1);
            }
        }
        private async void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            var bgclr = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            SolidColorBrush asbr = (SolidColorBrush)AppearanceStack.Background;
            try
            {
                if (LowerOpacityThread.IsAlive || RaiseOpacityThread.IsAlive)
                {
                    return;
                }
            }
            catch (Exception)
            {

            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (asbr.Color.A == bgclr.Color.A)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            while (LowerOpacityThread.IsAlive) { }
                        }
                        catch (Exception)
                        {

                        }
                    });
                        LowerOpacityThread = new Thread(LowerOpacity);
                    LowerOpacityThread.Start();
                }
            }
            else
            {
                if (asbr.Color.A != bgclr.Color.A)
                {
                    await Task.Run(() =>
                    {
                        try 
                        { 
                            while (RaiseOpacityThread.IsAlive) { }
                        }
                        catch (Exception)
                        {

                        }
                    });
                    RaiseOpacityThread = new Thread(RaiseOpacity);
                    RaiseOpacityThread.Start();
                }
            }
        }
        private void OpenDebugButton_Click(object sender, RoutedEventArgs e)
        {
            debugPanel = new DebugPanel();
            debugPanel.Show();
        }
        private void OpenPacketTesterButton_Click(object sender, RoutedEventArgs e)
        {
            PacketTester tester = new PacketTester();
            tester.Show();
        }

        // Messaging column functions
        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberBar.Visibility == Visibility.Visible)
            {
                MemberBar.Visibility = Visibility.Collapsed;
                ServerButtonsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MemberBar.Visibility = Visibility.Visible;
                ServerButtonsGrid.Visibility = Visibility.Collapsed;
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.ShowError("Test", "An error has occurred in some random bullshit\nTry doing something about it idk.", new byte[3] {1,1,1 });
            Debug.WriteLog("Test", "An error has occurred in some random bullshit\nTry doing something about it idk.");
        }

        // Test functions
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

        // Error Popup Functions
        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorPopup.Visibility = Visibility.Collapsed;
        }
        private void CloseAppButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenLogButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Debug.CurrentFilePath);
        }
    }
}
