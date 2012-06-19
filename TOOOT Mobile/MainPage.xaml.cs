using System;
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

namespace TOOOT_Mobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
                App.ViewModel.LoadData();

            if (Session.Emp.StartDate == DateTime.Today)
                NavigationService.Navigate(new Uri("/NewHire.xaml", UriKind.Relative));  
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            var newE = new Event(DateTime.Now,8,"A Day Off");
            Session.Emp.Events.Add(newE);
            Session.EventForEditing = newE;
            NavigationService.Navigate(new Uri("/EventEdit.xaml", UriKind.Relative));
        }

        private void ListBoxItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Session.EventForEditing = (Event)Log.SelectedItem;
            NavigationService.Navigate(new Uri("/EventEdit.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewHire.xaml", UriKind.Relative));
        }
    }
}