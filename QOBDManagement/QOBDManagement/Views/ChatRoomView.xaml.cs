using QOBDCommon.Classes;
using QOBDManagement.Classes;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using QOBDManagement.ViewModel;
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

namespace QOBDManagement.Views
{
    /// <summary>
    /// Interaction logic for ChatRoomView.xaml
    /// </summary>
    public partial class ChatRoomView : UserControl, IChatRoom
    {
        private int _offset = -1;
        private const int _maxMessage = 3;
        private List<UIElement> _messageHistory;

        public ChatRoomView()
        {
            InitializeComponent();
            _messageHistory = new List<UIElement>();
        }

        private void ChatRoomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext as DiscussionViewModel != null)
            {
                ((DiscussionViewModel)this.DataContext).ChatRoom = this;
                ((DiscussionViewModel)this.DataContext).load();
            }
            tbxMessage.Focus();
        }

        public async void showMyReply(MessageModel messageModel, bool isNewDiscussion = false)
        {
            if (Application.Current != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    displayAuthenticatedUserMessage(messageModel, isNewDiscussion);
                }
                else
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        displayAuthenticatedUserMessage(messageModel, isNewDiscussion);
                    }));
            }
        }

        private void displayAuthenticatedUserMessage(MessageModel messageModel, bool isNewDiscussion = false)
        {
            int cpt = chatRoomZone.Children.Count;
            Button btnMessage = new Button();
            btnMessage.Uid = messageModel.TxtDate;
            btnMessage.Name = "btnMessage_" + cpt;
            btnMessage.HorizontalAlignment = HorizontalAlignment.Right;
            btnMessage.Width = 300;

            btnMessage.Style = (Style)FindResource("Reply");
            btnMessage.Content = messageLayout(messageModel);
            _messageHistory.Add(btnMessage);
            displayMessage(btnMessage);
        }

        public async void showRecipientReply(MessageModel messageModel, bool isNewDiscussion = false)
        {
            if (Application.Current != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    displayRecipientMessage(messageModel, isNewDiscussion);
                }
                else
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        displayRecipientMessage(messageModel, isNewDiscussion);
                    }));
            }
        }

        private void displayRecipientMessage(MessageModel messageModel, bool isNewDiscussion = false)
        {
            int cpt = chatRoomZone.Children.Count;
            Button btnMessage = new Button();

            btnMessage.Width = 300;
            btnMessage.HorizontalAlignment = HorizontalAlignment.Left;
            btnMessage.Name = "btnMessage_" + cpt;

            btnMessage.Uid = messageModel.TxtDate;
            btnMessage.Style = (Style)FindResource("RecipientReply");
            btnMessage.Content = messageLayout(messageModel);
            _messageHistory.Add(btnMessage);
            displayMessage(btnMessage);
        }

        private UIElement messageLayout(MessageModel messageModel)
        {
            int cpt = chatRoomZone.Children.Count;
            Grid grid = new Grid();
            grid.Name = "grd_" + cpt;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            // user name layout
            TextBlock txtUserName = new TextBlock();
            txtUserName.Text = messageModel.TxtUserName;
            txtUserName.FontSize = 12;
            txtUserName.Foreground = new SolidColorBrush(Colors.DarkGray);
            txtUserName.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(txtUserName, 0);
            Grid.SetColumnSpan(txtUserName, 2);
            grid.Children.Add(txtUserName);

            // message layout
            TextBlock txtMessage = new TextBlock();
            txtMessage.Text = messageModel.TxtContent;// Utility.stringSpliter(messageModel.TxtContent, 15);
            txtMessage.FontSize = 14.667;
            txtMessage.TextWrapping = TextWrapping.Wrap;
            Grid.SetRow(txtMessage, 1);
            Grid.SetColumnSpan(txtMessage, 2);
            grid.Children.Add(txtMessage);

            // date layout
            TextBlock txtDate = new TextBlock();
            txtDate.Text = messageModel.TxtDate;
            txtDate.FontSize = 12;
            txtDate.Foreground = new SolidColorBrush(Colors.DarkGray);
            txtDate.VerticalAlignment = VerticalAlignment.Bottom;
            txtDate.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetRow(txtDate, 2);
            Grid.SetColumn(txtDate, 1);
            grid.Children.Add(txtDate);

            return grid;
        }

        public async void showInfo(MessageModel messageModel)
        {
            if (Application.Current != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    displayInformation(messageModel);
                }
                else
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        displayInformation(messageModel);
                    }));
            }
        }

        private void displayInformation(MessageModel messageModel)
        {
            int cpt = chatRoomZone.Children.Count;
            TextBlock txtBlock = new TextBlock();
            txtBlock.Name = "txtErrMessage_" + cpt;// ((messageModel.Message.ID != 0) ? messageModel.TxtID : cpt.ToString());
            txtBlock.Uid = messageModel.TxtDate;
            txtBlock.HorizontalAlignment = HorizontalAlignment.Center;
            txtBlock.Text = messageModel.TxtContent;
            txtBlock.TextWrapping = TextWrapping.Wrap;
            chatRoomZone.Children.Add(txtBlock);
            _messageHistory.Add(txtBlock);
        }

        private void displayMessage(UIElement message)
        {
            populateMessageZone(_messageHistory.OrderByDescending(x => Utility.convertToDateTime(x.Uid)).Take(_maxMessage).ToList());
        }

        private async void populateMessageZone(List<UIElement> messageList)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                chatRoomZone.Children.Clear();
                foreach (UIElement message in messageList.OrderBy(x => Utility.convertToDateTime(x.Uid)).ToList())
                {
                    chatRoomZone.Children.Add(message);
                }
                chatRoomZone.UpdateLayout();
            }
            else
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    chatRoomZone.Children.Clear();
                    foreach (UIElement message in messageList.OrderBy(x => Utility.convertToDateTime(x.Uid)).ToList())
                    {
                        if(!chatRoomZone.Children.Contains(message))
                            chatRoomZone.Children.Add(message);
                    }
                    chatRoomZone.UpdateLayout();
                }));
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;

            if (scrollViewer.VerticalOffset == 0 && ((_maxMessage - 1) * _offset + _maxMessage) <= _messageHistory.Count())
            {
                if (_offset > 0)
                {
                    populateMessageZone(_messageHistory.OrderByDescending(x => Utility.convertToDateTime(x.Uid)).Skip((_maxMessage - 1) * _offset).Take(_maxMessage).ToList());
                    svChatRoom.ScrollToVerticalOffset(10);
                    _offset++;
                }
                else
                {
                    _offset = 0;
                    svChatRoom.ScrollToVerticalOffset(svChatRoom.ScrollableHeight);
                }
            }

            else if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                if (_offset >= 0)
                {
                    populateMessageZone(_messageHistory.OrderByDescending(x => Utility.convertToDateTime(x.Uid)).Skip((_maxMessage - 1) * _offset).Take(_maxMessage).ToList());
                    svChatRoom.ScrollToVerticalOffset(svChatRoom.ScrollableHeight - 10);

                    if (_offset > 0)
                        _offset--;
                    else
                        _offset++;
                }
            }

        }
    }
}
