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
            UIContext dataContext = new UIContext();
            if (dataContext.setChatWindowContext(this) != null)
            {
                ((ChatRoomViewModel)this.DataContext).DiscussionViewModel.ChatRoom = this;
                ((ChatRoomViewModel)this.DataContext).DiscussionViewModel.load();
            }
            tbxMessage.Focus();
        }

        public async void showMyReply(MessageModel messageModel, bool isNewDiscussion = false)
        {
            if (Application.Current != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    int cpt = chatRoomZone.Children.Count;
                    Button btnMessage = new Button();
                    btnMessage.Width = 300;
                    btnMessage.HorizontalAlignment = HorizontalAlignment.Right;
                    btnMessage.Uid = messageModel.TxtDate;
                    btnMessage.Name = "btnMessage_" + cpt;// ((messageModel.Message.ID != 0) ? messageModel.TxtID : cpt.ToString());
                    TextBlock txtBlock = new TextBlock();
                    txtBlock.Text = messageModel.TxtContent;
                    btnMessage.Style = (Style)FindResource("Reply");
                    btnMessage.Content = txtBlock;
                    //chatRoomZone.Children.Add(btnMessage);
                    _messageHistory.Add(btnMessage);
                    displayMessage(btnMessage);
                }
                else
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        int cpt = chatRoomZone.Children.Count;
                        Button btnMessage = new Button();
                        btnMessage.Width = 300;
                        btnMessage.HorizontalAlignment = HorizontalAlignment.Right;
                        btnMessage.Uid = messageModel.TxtDate;
                        btnMessage.Name = "btnMessage_" + cpt;// ((messageModel.Message.ID != 0) ? messageModel.TxtID : cpt.ToString());
                        TextBlock txtBlock = new TextBlock();
                        txtBlock.Text = messageModel.TxtContent;
                        btnMessage.Style = (Style)FindResource("Reply");
                        btnMessage.Content = txtBlock;
                        //chatRoomZone.Children.Add(btnMessage);
                        _messageHistory.Add(btnMessage);
                        displayMessage(btnMessage);
                    }));
            }
        }

        public async void showInfo(MessageModel messageModel)
        {
            if (Application.Current != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    int cpt = chatRoomZone.Children.Count;
                    TextBlock txtBlock = new TextBlock();
                    txtBlock.Name = "txtErrMessage_" + cpt;// ((messageModel.Message.ID != 0) ? messageModel.TxtID : cpt.ToString());
                    txtBlock.Uid = messageModel.TxtDate;
                    txtBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    txtBlock.Text = messageModel.TxtContent;
                    chatRoomZone.Children.Add(txtBlock);
                    _messageHistory.Add(txtBlock);
                }
                else
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        int cpt = chatRoomZone.Children.Count;
                        TextBlock txtBlock = new TextBlock();
                        txtBlock.Name = "txtErrMessage_" + cpt;// ((messageModel.Message.ID != 0) ? messageModel.TxtID : cpt.ToString());
                        txtBlock.Uid = messageModel.TxtDate;
                        txtBlock.HorizontalAlignment = HorizontalAlignment.Center;
                        txtBlock.Text = messageModel.TxtContent;
                        chatRoomZone.Children.Add(txtBlock);
                        _messageHistory.Add(txtBlock);
                    }));
            }
        }

        public async void showRecipientReply(MessageModel messageModel, bool isNewDiscussion = false)
        {
            if (Application.Current != null)
            {
                if (Application.Current.Dispatcher.CheckAccess())
                {
                    int cpt = chatRoomZone.Children.Count;
                    Button btnMessage = new Button();
                    btnMessage.Width = 300;
                    btnMessage.HorizontalAlignment = HorizontalAlignment.Left;
                    btnMessage.Name = "btnMessage_" + cpt;// cpt;
                    btnMessage.Uid = messageModel.TxtDate;
                    TextBlock txtBlock = new TextBlock();
                    txtBlock.Name = "txtMessage_" + cpt;//  ((messageModel.Message.ID != 0) ? messageModel.TxtID : cpt.ToString());
                    txtBlock.Text = messageModel.TxtContent;
                    btnMessage.Style = (Style)FindResource("RecipientReply");
                    btnMessage.Content = txtBlock;
                    //chatRoomZone.Children.Add(btnMessage);
                    _messageHistory.Add(btnMessage);
                    displayMessage(btnMessage);
                }
                else
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        int cpt = chatRoomZone.Children.Count;
                        Button btnMessage = new Button();
                        btnMessage.Width = 300;
                        btnMessage.HorizontalAlignment = HorizontalAlignment.Left;
                        btnMessage.Name = "btnMessage_" + cpt;
                        btnMessage.Uid = messageModel.TxtDate;
                        TextBlock txtBlock = new TextBlock();
                        txtBlock.Name = "txtMessage_" + cpt;//  ((messageModel.Message.ID != 0) ? messageModel.TxtID : cpt.ToString());
                        txtBlock.Text = messageModel.TxtContent;
                        btnMessage.Style = (Style)FindResource("RecipientReply");
                        btnMessage.Content = txtBlock;
                        //chatRoomZone.Children.Add(btnMessage);
                        _messageHistory.Add(btnMessage);
                        displayMessage(btnMessage);
                    }));
            }
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
