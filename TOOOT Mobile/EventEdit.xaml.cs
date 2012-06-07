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
    public partial class EventEdit : PhoneApplicationPage
    {
        public EventEdit()
        {
            InitializeComponent();
            prefillListPickers();
            NameBox.Text = Session.EventForEditing.Description;
            HourBox.SelectedItem = Session.EventForEditing.Hours;
            DateBox.Value = Session.EventForEditing.Date;
            //TypeBox.SelectedItem = Session.EventForEditing.Category.ToString();
        }

        private void prefillListPickers()
        {
            var hourChoices = new List<double>() { 4, 8, 16, 24, 40 };
            foreach (var choice in hourChoices)
                if (!HourBox.Items.Contains(choice))
                    HourBox.Items.Add(choice);

            //var catChoices = new List<string>() { "PaidTimeOff", "Holiday", "Illness" };
            //foreach (var choice in catChoices)
            //    if (!TypeBox.Items.Contains(choice))
            //        TypeBox.Items.Add(choice);
        }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            Session.EventForEditing.Description = NameBox.Text;
            Session.EventForEditing.Hours = (double)HourBox.SelectedItem;
            //Session.EventForEditing.Category = (TimeCategory)Enum.Parse(typeof(TimeCategory), TypeBox.SelectedItem.ToString(), true);
            Session.EventForEditing.Date = (DateTime)DateBox.Value;
            App.ViewModel.TriggerCompleteUIRefresh();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarDeleteButton_Click(object sender, EventArgs e)
        {
            Session.Emp.Events.Remove(Session.EventForEditing);
            App.ViewModel.TriggerCompleteUIRefresh();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarCancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}