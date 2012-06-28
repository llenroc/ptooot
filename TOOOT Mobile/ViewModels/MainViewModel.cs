using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Xml.Serialization;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Windows.Navigation;


namespace TOOOT_Mobile
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            LoadData();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public List<Event> Items { get { return Session.Emp.Events; } }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (IsolatedStorageFile.GetUserStoreForApplication().FileExists("tooot.xml"))
            {
                var fileStream = IsolatedStorageFile.GetUserStoreForApplication().OpenFile("tooot.xml", System.IO.FileMode.Open);
                var cerealizer = new XmlSerializer(typeof(Employee));
                Session.Emp = (Employee)cerealizer.Deserialize(fileStream);
                fileStream.Close();
                Session.Emp.Recompute();
                this.IsDataLoaded = true;
            }
            else
            {
                Session.Emp = new Employee("New Guy", DateTime.Today);
                Session.Emp.Save("tooot.xml");
                //var uri = new Uri("/NewUserPage.xaml", UriKind.Relative);
                //(Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);
            }
        }

        public string StatusText
        {
            get
            {
                if (IsDataLoaded)
                    return (Session.Emp.PaidTimeOff).ToString() + " Days Left";
                else
                    return "-- Days Left";
            }
        }

        public DateTime StartDate
        {
            get
            {
                return Session.Emp.StartDate;
            }
            set
            {
                Session.Emp.StartDate = value;
                TriggerCompleteUIRefresh();
            }
        }

        public bool ShowAds
        {
            get { return Session.Emp.EnableAds; }
            set { Session.Emp.EnableAds = value; }
        }

        public void TriggerCompleteUIRefresh()
        {
            Session.Emp.Recompute();
            Session.Emp.Save("tooot.xml");
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Rules
        {
            get
            {
                return @"
Holidays   
    Aside from the 8 Company Holidays,
    Employees can choose 1 more holiday.
    This is availible to new hires who 
    started the first half of the year.

PTO        
    You can calculate this year's PTO
    in days with this formula:

    16 + (.5 * number of years worked)
    + Min(previous year's PTO, 10)

Illness    
     30 consecutive calendar days per incident.
             
";
            }
        }

        public Event SelectedLogEntry { get; set; }
    }
}