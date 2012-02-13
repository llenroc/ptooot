using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TOOOT_Mobile
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public ItemViewModel() { }
        public ItemViewModel(Event ptoEvent)
        {
            _event = ptoEvent;
            LineOne = ptoEvent.Description;
        }

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineOne
        {
            get
            {
                return _event.Description;
            }
            set
            {
                _event.Description = value;
                NotifyPropertyChanged("LineOne");
            }
        }

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineTwo
        {
            get
            {
                return _event.Category.ToString();
            }
            set
            {
                if (value != _event.Category.ToString())
                {
                     _event.Category = (TimeCategory) Enum.Parse(typeof(TimeCategory), value, true);
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree
        {
            get
            {
                return _event.Description;
            }
            set
            {
                if (value != _event.Description)
                {
                    _event.Description = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Event _event;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}