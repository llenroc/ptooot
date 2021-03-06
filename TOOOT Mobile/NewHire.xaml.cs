﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace TOOOT_Mobile
{
    public partial class NewHire : PhoneApplicationPage
    {
        public NewHire()
        {
            InitializeComponent();
            DataContext = Session.Emp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Session.Emp.StartDate = Date.Value.Value;
            App.ViewModel.TriggerCompleteUIRefresh();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "TimeOffTracker";
            emailComposeTask.Body = "";
            emailComposeTask.To = "owen@owenjohnson.info";

            emailComposeTask.Show();
        }
    }
}