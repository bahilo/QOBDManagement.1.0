﻿using QOBDManagement.Classes;
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
    /// Interaction logic for ChatHistoryView.xaml
    /// </summary>
    public partial class ChatHistoryView : UserControl
    {
        public ChatHistoryView()
        {
            InitializeComponent();
        }

        private async void ChatRoomHistoryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext dataContext = new DataContext();
            if (dataContext.setChatWindowContext(this) != null)
                await ((ChatRoomViewModel)this.DataContext).MessageViewModel.load();                            
        }
    }
}
