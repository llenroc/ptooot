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
    public partial class Rules : PhoneApplicationPage
    {
        public Rules()
        {
            InitializeComponent();
            DataContext = this;

        }

        public string PTORules
        {
            get
            {
                return @"
    You can calculate this year's PTO
    in days with this formula:

    16 + (.5 * number of years worked)
    + Min(previous year's PTO, 10)
";
            }
        }
        public string HolidayRules
        {
            get
            {
                return @"
    Aside from the 8 Company Holidays,
    Employees can choose 1 more holiday.
    This is availible to new hires who 
    started the first half of the year.
";
            }
        }

        public string SickRules
        {
            get
            {
                return @"
    Do not take more than 30 days per incident
";
            }
        }
    }
}