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
    /// Interaction logic for AgentDetailView.xaml
    /// </summary>
    public partial class AgentDetailView : UserControl
    {
        public AgentDetailView()
        {
            InitializeComponent();
        }

        private void AgentDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            UIContext dataContext = new UIContext();
            if (dataContext.setWindowContext(this) != null)
            {
                ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.load();
                pwdBox.Password = ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.SelectedAgentModel.TxtHashedPassword;
                pwdBoxVerification.Password = ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.SelectedAgentModel.TxtHashedPassword;
                pwdBox.LostFocus += ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.onPwdBoxPasswordChange_updateTxtClearPassword;
                pwdBoxVerification.LostFocus += ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.onPwdBoxVerificationPasswordChange_updateTxtClearPasswordVerification;
            }
        }
    }
}
