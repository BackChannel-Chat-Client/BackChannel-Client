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
using BackChannel.Enums;
using System.Text;
using System.Windows.Data;
using System.Globalization;

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
        public bool JoinProgress;

        // A reference to the debug panel when open for debug logging 
        DebugPanel debugPanel;

        // Initialization 
        public MainWindow()
        {
            InitializeComponent();
            DeclareVars();
            GetSettings();
            GetServers();
            Debug.CreateSessionID();
        }
        private void DeclareVars()
        {
            serverViewModel = new ServerViewModel(); // Instantiates the ViewModel
            channelViewModel = new ChannelViewModel(); // Instantiates the ViewModel
            chatViewModel = new ChatViewModel(); // Instantiates the ViewModel
            memberViewModel = new MemberViewModel(); // Instantiates the ViewModel
            ServerListView.ItemsSource = serverViewModel.Servers; // Set the Server List's Source to the ViewModel
            ChannelListView.ItemsSource = channelViewModel.Channels; // Set the Channel List's Source to the ViewModel
            ChatListView.ItemsSource = chatViewModel.Messages; // Set the Cannel List's Source to the ViewModel
            MemberListView.ItemsSource = memberViewModel.Members; // Set the Cannel List's Source to the ViewModel
            Packet.PacketQueue = new List<Packet>();
        }
        private void GetSettings()
        {
            // Not Implemented
        }
        private void GetServers()
        {

        }

        // Column resizing handlers
        private void LeftColumnStack_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
                Sets the left and middle column max, so that they dont go offscreen / cause the text pane to get to small
            */
            try
            {
                UserInfo.Visibility = Visibility.Collapsed;
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
                UserInfo.Visibility = Visibility.Collapsed;
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
                UserInfo.Visibility = Visibility.Collapsed;
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
        private void AddServerButton_Click(object sender, RoutedEventArgs e)
        {
            ServerJoin.Visibility = Visibility.Visible;
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
        private void CloseServerJoinButton_Click(object sender, RoutedEventArgs e)
        {
            ServerJoin.Visibility = Visibility.Collapsed;
            JoinProgress = false;
        }
        private void ServerJoinButton_Click(object sender, RoutedEventArgs e)
        {
            JoinProgress = true;
            Thread t = new Thread(StartProgressBars);
            t.Start();
            Thread b = new Thread(ConnectToNewServer);
            b.Start(false);
        }
        private void ConnectToNewServer(object AllowSelfSigned)
        {
            Packet GetChannels = new Packet();
            GetChannels.AllowSelfSigned = (bool)AllowSelfSigned;
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    GetChannels.AuthKey = Packet.ToByteArray("TestKey");
                    GetChannels.ChannelID = 0;
                    GetChannels.GeneratePID();
                    GetChannels.SetRequestType(RequestType.GetChannelList);
                    GetChannels.RequestBody = Packet.None;
                    GetChannels.GetPacketSize();
                    GetChannels.ServerIP = ServerIPEntry.Text;
                    GetChannels.ServerPort = Convert.ToInt16(ServerPortEntry.Text);
                }));

                var check = GetChannels.SendPacket();
                var response = GetChannels.RecvResponse();

                Server newServer = new Server();
                newServer.Name = GetChannels.ServerIP;
                newServer.Channels = new List<Channel>();
                while (true)
                {
                    uint ChannelID = (UInt32)BitConverter.ToUInt32(new byte[] { response.ResponseBody[0], response.ResponseBody[1], response.ResponseBody[2], response.ResponseBody[3], });

                    uint MaxMessages = (UInt32)BitConverter.ToUInt32(new byte[] { response.ResponseBody[4], response.ResponseBody[5], response.ResponseBody[6], response.ResponseBody[7], });

                    byte[] ChannelName = new byte[response.ResponseBody.Length - 8];

                    Buffer.BlockCopy(response.ResponseBody, 8, ChannelName, 0, response.ResponseBody.Length - 8);

                    Channel newChannel = new Channel();
                    newChannel.ID = (int)Convert.ToUInt32(ChannelID);
                    newChannel.MaxMessages = (int)Convert.ToUInt32(MaxMessages);
                    newChannel.IsText = Visibility.Visible;
                    newChannel.Name = Encoding.UTF8.GetString(ChannelName, 0, ChannelName.Length);

                    newServer.Channels.Add(newChannel);
                    if (response.ResponseStatus != (uint)ResponseStatus.MoreData)
                    {
                        break;
                    }
                    response = GetChannels.RecvResponse();

                }

                serverViewModel.AddServer(newServer);
                JoinProgress = false;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ServerJoin.Visibility = Visibility.Collapsed;
                    ErrorPopup.Visibility = Visibility.Collapsed;
                }));
            }
            catch (Exception err)
            {
                if (GetChannels.connectionError == ConnectionError.SelfSigned)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Debug.ShowError("Authentication", "This server's certificate is self signed.", DebugPopupType.Security);
                        ServerJoin.Visibility = Visibility.Collapsed;
                    }));
                }
                else if (GetChannels.connectionError == ConnectionError.CertError)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Debug.ShowError("Authentication", "Could't verify server auth", DebugPopupType.Notify);
                        ServerJoin.Visibility = Visibility.Collapsed;
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Debug.ShowError("Connection", $"Could't connect to the server\n{err.Message}", DebugPopupType.PossiblyFatal);
                        ServerJoin.Visibility = Visibility.Collapsed;
                    }));
                }
                JoinProgress = false;
            }
        }
        private void AllowButton_Click(object sender, RoutedEventArgs e)
        {
            ServerJoin.Visibility = Visibility.Visible;
            ErrorPopup.Visibility = Visibility.Collapsed;
            Thread t = new Thread(ConnectToNewServer);
            t.Start(true);
        }
        
        private void StartProgressBars()
        {
            try
            {
                while (JoinProgress)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBOne.IsIndeterminate = true;
                        PBOne.Visibility = Visibility.Visible;
                    }));
                    Thread.Sleep(400);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBFour.Visibility = Visibility.Visible;
                        PBFour.IsIndeterminate = false;
                    }));
                    Thread.Sleep(1000);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBTwo.IsIndeterminate = true;
                        PBTwo.Visibility = Visibility.Visible;
                    }));
                    Thread.Sleep(400);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBOne.Visibility = Visibility.Visible;
                        PBOne.IsIndeterminate = false;
                    }));
                    Thread.Sleep(1000);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBThree.IsIndeterminate = true;
                        PBThree.Visibility = Visibility.Visible;
                    }));
                    Thread.Sleep(400);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBTwo.Visibility = Visibility.Visible;
                        PBTwo.IsIndeterminate = false;
                    }));
                    Thread.Sleep(1000);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBFour.IsIndeterminate = true;
                        PBFour.Visibility = Visibility.Visible;
                    }));
                    Thread.Sleep(400);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PBThree.Visibility = Visibility.Visible;
                        PBThree.IsIndeterminate = false;
                    }));
                    Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {

            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                PBOne.Visibility = Visibility.Collapsed;
                PBTwo.Visibility = Visibility.Collapsed;
                PBThree.Visibility = Visibility.Collapsed;
                PBFour.Visibility = Visibility.Collapsed;
            }));
        }

        // Channel column functions
        private async void ChannelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UserInfo.Visibility = Visibility.Collapsed;
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
                    try
                    {
                        ChannelListView.SelectedItem = e.RemovedItems[0];
                    }
                    catch (Exception)
                    {
                        ChannelListView.SelectedItem = null;
                    }
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

        // Messaging column functions
        private void MembersButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberBar.Visibility == Visibility.Visible)
            {
                MemberBar.Visibility = Visibility.Collapsed;
                UserInfo.Visibility = Visibility.Collapsed;
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
            //Debug.ShowError("Test", "An error has occurred in some random bullshit\nTry doing something about it idk.", new byte[3] {1,1,1 });
            //Debug.WriteLog("Test", "An error has occurred in some random bullshit\nTry doing something about it idk.");
        }
        private void MemberListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(UserInfo.Visibility == Visibility.Visible)
            {
                UserInfo.Visibility = Visibility.Collapsed;
                return;
            }
            try
            {
                var point = Mouse.GetPosition(MemberListView);
                UserInfoPanel.Margin = new Thickness(10, 50+point.Y, MemberBar.ActualWidth, 10);
                UserInfo.Visibility = Visibility.Visible;
                //debugPanel.UpdateMousePos($"({point.X},{point.Y})");
                //MemberListView.SelectedItem = null;
            }
            catch (Exception)
            {
                UserInfo.Visibility = Visibility.Collapsed;
            }
        }
        private void UserInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MemberListView.SelectedItem = null;
        }

        // Error Popup Functions
        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorPopup.Visibility = Visibility.Collapsed;
            JoinProgress = false;
        }
        private void CloseAppButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
