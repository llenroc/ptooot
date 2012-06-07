using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TOOOT_Mobile
{
    //public enum TimeCategory
    //{
    //    PaidTimeOff,
    //    Holiday,
    //    Illness
    //}

    /// <summary>
    /// A single time off event measured in hours.
    /// </summary>
    public class Event : INotifyPropertyChanged
    {
        private DateTime _date;
        [XmlElement]
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                NotifyChanged(string.Empty);
            }
        }
        //private TimeCategory _cat;
        //[XmlElement]
        //public TimeCategory Category
        //{
        //    get
        //    {
        //        return _cat;
        //    }
        //    set
        //    {
        //        _cat = value;
        //        NotifyChanged(string.Empty);
        //    }
        //}

        private double _hours;
        [XmlElement]
        public double Hours
        {
            get
            {
                return _hours;
            }
            set
            { _hours = value; NotifyChanged(string.Empty); }

        }
        private string _desc;
        [XmlElement]
        public string Description { get { return _desc; } set { _desc = value; NotifyChanged(null); } }

        public Event() { Date = DateTime.Now; }

        public Event(DateTime date, double hours, string desc)
        {
            Date = date;
            Hours = hours;
            Description = desc;
        }

        public void NotifyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
