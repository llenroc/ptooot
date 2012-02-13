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
        public int Tenure { get; set; }
        [XmlIgnore]
        public double PaidTimeOff { get; set; }
        [XmlIgnore]
        public double Illness { get; set; }
        [XmlIgnore]
        public double Holiday { get; set; }
        #endregion

        /// <summary>
        /// The start date of the employee.  Don't change this once set since YearDetails are based off it.
        /// Can't be readonly because the XML serializer needs to set it.
        /// </summary>
        /// 
        [XmlElement]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Employee name.  Useful for the UI, but not in the model code.
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// PTO events are grouped by years.
        /// </summary>
        [XmlElement]
        public List<YearDetails> Years { get; set; }

        /// <summary>
        /// Don't use unless you're sure you know what you're doing.  Defined for [Serializable].
        /// </summary>
        public Employee() { }

        /// <summary>
        /// Default constructor.  Initializes the start date and the active YearDetails objects.
        /// </summary>
        /// <param name="name">Employee name.</param>
        /// <param name="startDate">Start date of the employee.  Don't change it once set.</param>
        public Employee(string name, DateTime startDate)
        {
            Name = name;
            StartDate = startDate;
            Years = new List<YearDetails>();
            ResetTotals();
            ValidateYears();
            Recompute();
        }

        /// <summary>
        /// Easy access to the active year to add new events.
        /// </summary>
        public YearDetails ThisYear
        {
            get
            {
                return Years.Last();
            }
        }

        private void ResetTotals()
        {
            Tenure = 0;
            Illness = 0;
            Holiday = 0;
            PaidTimeOff = 0;
        }

        /// <summary>
        /// Adds missing years if the app hasn't been used, or a new year that needs to be added from an anniversary.
        /// </summary>
        public void ValidateYears()
        {
            List<int> missingYears = new List<int>();

            for (int year = StartDate.Year; year <= DateTime.Now.Year; year++)
                missingYears.Add(year);

            foreach (int year in Years.Select(detail => detail.Start.Year))
                missingYears.Remove(year);

            foreach (int year in missingYears)
                Years.Add(new YearDetails(StartDate.AddDays(365)));

            Years.Sort((d1, d2) => d1.Start.Year.CompareTo(d2.Start.Year));
        }

        /// <summary>
        /// Call to recalculate the summary hours after events have been added.
        /// </summary>
        public void Recompute()
        {
            ResetTotals();
            foreach (YearDetails year in Years)
                year.ApplyTo(this);
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
    }
}
