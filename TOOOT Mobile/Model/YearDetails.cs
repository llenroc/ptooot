using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TOOOT_Mobile
{
    /// <summary>
    /// Precomputed bundle of events for a given year.  The year may or may not be over.
    /// </summary>
    public class YearDetails
    {
        #region Starting allocations for all employees.
        public const double InitialPaidTimeOff = 16 * 8;
        public const double MaxCarryover = 10 * 8;
        public const double HolidayTime = 1 * 8;
        public const double TenureBonus = 4;
        #endregion

        /// <summary>
        /// The start date of the year.  Should be the anniversary month and day of the employee with a different year.
        /// </summary>
        [XmlElement]
        public DateTime Start;

        /// <summary>
        /// A list of PTO, Illness, or Holidays that will be subtracted or added from the Employee.
        /// </summary>
        [XmlElement]
        public ObservableCollection<Event> Events { get; set; }

        /// <summary>
        /// Don't use.  Defined for [Serializable].
        /// </summary>
        public YearDetails() { Events = new ObservableCollection<Event>(); }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="start">The start date of the year.  Month and Day should be the anniversary.</param>
        public YearDetails(DateTime start)
        {
            Start = start;
            Events = new ObservableCollection<Event>();
        }

        /// <summary>
        /// Apply the constraints, additional PTO, and new events of the year to an Employee totals.
        /// </summary>
        /// <param name="e">Employee that hasn't had the year applied yet.</param>
        public void ApplyTo(Employee employee)
        {
            UpdateAnnualAllocations(employee);
            ApplyEvents(employee);
        }

        /// <summary>
        /// Setter to add a new out of office Event to the current year.
        /// </summary>
        /// <param name="e">Fully configured PTO, Illness, or Holiday.</param>
        public void AddEvent(Event e)
        {
            Events.Add(e);
            // If we wanted to, we could add the Recompute() here and not require a manual call.
            // Except that it might cause churn depending where AddEvent() ends up being called. 
        }

        /// <summary>
        /// Adjust the master Employee object with the new PTO, holidays, and custom events from the year.
        /// CAUTION: Applying the events of the same year twice to the employee will adjust the numbers a second
        ///          time and make the Employee totals wrong.
        /// </summary>
        /// <param name="employee">The employee to adjust.</param>
        private void ApplyEvents(Employee employee)
        {
            foreach (Event e in Events)
            {
                switch (e.Category)
                {
                    case TimeCategory.Holiday:
                        employee.Holiday -= e.Hours;
                        break;
                    case TimeCategory.PaidTimeOff:
                        employee.PaidTimeOff -= e.Hours;
                        break;
                    case TimeCategory.Illness:
                        employee.Illness += e.Hours;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Category");
                }
            }
        }

        /// <summary>
        /// Apply a year of Pioneer's screwy PTO, Illness, and Holiday logic to an Employee.
        /// </summary>
        /// <param name="e"></param>
        private void UpdateAnnualAllocations(Employee e)
        {
            // Let's reset our illness counter every year.
            e.Illness = 0;

            // Bump tenure by a year if this YearDetails is not the first year.
            if (Start.Year > e.StartDate.Year)
                e.Tenure++;

            // Hiring rules
            // 1. Maximum of 10 days can be carried over on the anniversary date.
            e.PaidTimeOff = Math.Min(e.PaidTimeOff, MaxCarryover);

            // 2. One selectable holiday per year unless it's the special case first year.
            // Rule: If the start date is before July 1st, then a holiday the first year.
            if (Start.Year > e.StartDate.Year || e.StartDate.Month < 7)
                e.Holiday = HolidayTime;
            else
                e.Holiday = 0;

            // 3. PTO Initial days + 0.5 days * completed years.
            e.PaidTimeOff += InitialPaidTimeOff + (TenureBonus * e.Tenure);
        }
    }
}
