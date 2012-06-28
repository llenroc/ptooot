using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.IO.IsolatedStorage;

namespace TOOOT_Mobile
{
    /// <summary>
    /// The root XML object of a single employee.  Includes name, start date, summary stats and PTO events.
    /// </summary>
    [XmlRoot]
    public class Employee
    {

        #region To keep the math accurate always Compute() from the year details.
        [XmlIgnore]
        public double Tenure { get; set; }
        [XmlIgnore]
        public double PaidTimeOff { get; set; }
        #endregion

        /// <summary>
        /// The start date of the employee.  Don't change this once set since YearDetails are based off it.
        /// Can't be readonly because the XML serializer needs to set it.
        /// </summary>
        /// 
        [XmlElement]
        public DateTime StartDate { get; set; }

        [XmlElement]
        public List<Event> Events { get; set; }

        /// <summary>
        /// Don't use unless you're sure you know what you're doing.  Defined for [Serializable].
        /// </summary>
        public Employee() { Events = new List<Event>(); }

        /// <summary>
        /// Default constructor.  Initializes the start date and the active YearDetails objects.
        /// </summary>
        /// <param name="name">Employee name.</param>
        /// <param name="startDate">Start date of the employee.  Don't change it once set.</param>
        public Employee(string name, DateTime startDate)
        {
            StartDate = startDate;
            Events = new List<Event>();
            Recompute();
        }

        /// <summary>
        /// Call to recalculate the summary hours after events have been added.
        /// </summary>
        public void Recompute()
        {
            var RollOverDate = StartDate.AddDays(365);

            PaidTimeOff = 16;
            Tenure = 0;

            Events.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));

            foreach (var ptoEvent in Events)
            {
                if (ptoEvent.Date > RollOverDate)
                {
                    Tenure += 0.5;
                    PaidTimeOff = Math.Min(PaidTimeOff, 10) + 16 + Tenure;
                    RollOverDate = RollOverDate.AddDays(365);
                }
                PaidTimeOff -= ptoEvent.Hours /8;
            }

            if (RollOverDate < DateTime.Now)
            {
                Tenure += .5;
                PaidTimeOff = Math.Min(PaidTimeOff, 10) + 16 + Tenure;
            }

            Events.Reverse();
        }

        /// <summary>
        /// Export the employee to an XML file.
        /// </summary>
        /// <param name="filename">The full path and filename.</param>
        public void Save(string filename)
        {
            var file = IsolatedStorageFile.GetUserStoreForApplication().CreateFile("tooot.xml");
            var serializer = new XmlSerializer(typeof(Employee));
            serializer.Serialize(file, this);
            file.Flush();
            file.Close();
        }

        #region Static Methods
        /// <summary>
        /// Export an employee to an XML file.
        /// </summary>
        /// <param name="e">The Employee object to export.</param>
        /// <param name="filename">The full path and filename.</param>
        static public void Save(Employee e, string filename)
        {
            e.Save(filename);
        }

        /// <summary>
        /// Load an XML serialized Employee object from a file.
        /// </summary>
        /// <param name="filename">The full path and filename.</param>
        /// <returns>Reconstituted object.</returns>
        static public Employee Load(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Employee));
            FileStream fs = new FileStream(filename, FileMode.Open);
            Employee result;
            result = (Employee)serializer.Deserialize(fs);
            fs.Close();
            result.Recompute();
            return result;
        }
        #endregion

        public bool EnableAds { get; set; }
    }
}
