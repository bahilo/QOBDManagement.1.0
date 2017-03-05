﻿using QOBDManagement.Classes;
using QOBDManagement.Interfaces;
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
        private const int maxMessage = 5;
        
        public ChatRoomView()
        {
            InitializeComponent();
        }

        private void ChatRoomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext dataContext = new DataContext();
            if (dataContext.setChatWindowContext(this) != null)
            {
                ((ChatRoomViewModel)this.DataContext).DiscussionViewModel.ChatRoom = this;
                ((ChatRoomViewModel)this.DataContext).DiscussionViewModel.load();
            }
        }

        public async void showMyReply(string message, bool isNewDiscussion = false)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                int cpt = chatRoomZone.Children.Count;
                Button btnMessage = new Button();
                btnMessage.Width = 300;
                btnMessage.HorizontalAlignment = HorizontalAlignment.Right;
                btnMessage.Name = "btnMessage" + cpt;
                TextBlock txtBlock = new TextBlock();
                txtBlock.Text = message;
                btnMessage.Style = (Style)FindResource("Reply");
                btnMessage.Content = txtBlock;
                chatRoomZone.Children.Add(btnMessage);
                displayMaxOfMessage();
            }));
        }

        public async void showInfo(string message)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                int cpt = chatRoomZone.Children.Count;
                TextBlock txtBlock = new TextBlock();
                txtBlock.Name = "btnErrMessage" + cpt;
                txtBlock.HorizontalAlignment = HorizontalAlignment.Center;
                txtBlock.Text = message;
                chatRoomZone.Children.Add(txtBlock);
            }));
        }

        public async void showRecipientReply(string message, bool isNewDiscussion = false)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                int cpt = chatRoomZone.Children.Count;
                Button btnMessage = new Button();
                btnMessage.Width = 300;
                btnMessage.HorizontalAlignment = HorizontalAlignment.Left;
                btnMessage.Name = "btnMessage" + cpt;
                TextBlock txtBlock = new TextBlock();
                txtBlock.Name = "txtMessage" + cpt;
                txtBlock.Text = message;
                btnMessage.Style = (Style)FindResource("RecipientReply");
                btnMessage.Content = txtBlock;
                chatRoomZone.Children.Add(btnMessage);
                displayMaxOfMessage();
            }));
        }

        private async void displayMaxOfMessage()
        {
            if (chatRoomZone.Children.Count > maxMessage)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    chatRoomZone.Children.RemoveRange(0, chatRoomZone.Children.Count - maxMessage);
                }));
            }
        }


    }
}
