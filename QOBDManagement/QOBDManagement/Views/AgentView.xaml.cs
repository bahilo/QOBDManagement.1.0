﻿using QOBDManagement.Classes;
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
    /// Interaction logic for AgentView.xaml
    /// </summary>
    public partial class AgentView : UserControl
    {
        public AgentView()
        {
            InitializeComponent();
        }

        private async void AgentView_Loaded(object sender, RoutedEventArgs e)
        {
            UIContext dataContext = new UIContext();
            if (dataContext.setWindowContext(this) != null)
            {
                if (!((MainWindowViewModel)this.DataContext).IsThroughContext)
                    await ((MainWindowViewModel)this.DataContext).AgentViewModel.loadAgents();
            }
        }
    }
}
