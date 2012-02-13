using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Xml.Serialization;
using Microsoft.Phone.Controls;


namespace TOOOT_Mobile
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<Event>();
            LoadData();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<Event> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

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
                Items = Session.Emp.ThisYear.Events;
                this.IsDataLoaded = true;
            }
            else
            {
                Session.Emp = new Employee("New Guy", DateTime.Now);
                Session.Emp.Save("tooot.xml");
                //var uri = new Uri("/NewUserPage.xaml", UriKind.Relative);
                //(Application.Current.RootVisual as PhoneApplicationFrame).Navigate(uri);
            }
        }

        public string RemainingPTO
        {
            get
            {
                if (IsDataLoaded)
                    return (Session.Emp.PaidTimeOff / 8).ToString() + " Days";
                else
                    return "-- Days";
            }
        }

        public string UsedOOO
        {
            get
            {
                return (Session.Emp.Illness / 8).ToString() + " Days";
            }
        }

        public string RemainingHoliday
        {
            get
            {
                return (Session.Emp.Holiday / 8).ToString() + " Days";
            }
        }

        public string EmpName
        {
            get
            {
                return Session.Emp.Name;
            }
            set
            {
                Session.Emp.Name = value;
                Session.Emp.Save("tooot.xml");
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

        public void TriggerCompleteUIRefresh()
        {
            Session.Emp.Recompute();
            Session.Emp.Save("tooot.xml");
            NotifyPropertyChanged(string.Empty);
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
Pioneer holiday, personal, and sick time rules	

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

        internal void AddNewItem()
        {
            Items.Add(new Event(DateTime.Now,TimeCategory.PaidTimeOff,8,"Day Off"));
            TriggerCompleteUIRefresh();
        }

        public Event SelectedLogEntry { get; set; }
    }
}