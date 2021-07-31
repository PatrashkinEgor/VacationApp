using System;
using System.Collections.Generic;
using System.Linq;
using VacationApp.Models;

namespace VacationApp.Services
{
    /// <summary>
    /// The class creates vacation instances, keeps a list of all vacations 
    /// and a vacation calendar that allows you to quickly find the vacations 
    /// that exist on the requested days.
    /// </summary>
    class VacationService
    {
        /// <summary>
        /// Contains Id lists of vacations existing on specific days.
        /// Lets you get a list of vacations for a specific day with O(1) complexity;
        /// </summary>
        private readonly Dictionary<DateTime, List<int>> _vacationCalendar = new ();

        /// <summary>
        /// Contains all vacations that was creating by instance of VacationService.
        /// Lets you get an instance of vacation by Id with O(1) complexity;
        /// </summary>
        private readonly Dictionary<int, Vacation> _vacations = new ();

        private readonly Random _random = new();
        private int _nextVacationId = 0;

        /// <summary>
        /// Creates a vacation instance, puts it in storage and "registers" it in the 
        /// vacation calendar.If the desired vacation dates intersect with the 
        /// employee's existing vacation, the function returns false, the vacation 
        /// creation is terminated.
        /// </summary>
        /// <param name="employee"> The employee for whom the vacation is being created</param>
        /// <param name="begin">Start of vacation</param>
        /// <param name="duration">Vacation duration</param>
        /// <returns> False if the employee already has a vacation during this period</returns>
        public bool AddVacation(Employee employee, DateTime begin, int duration)
        {
            
            var intersections = GetVacations(begin, duration);
            foreach (var inter in intersections)
            {
                if (inter.Employee.Id == employee.Id)
                    return false;
            }
            
            var vacation = new Vacation()
            {
                Employee = employee,
                Begin = begin,
                End = begin.AddDays(duration),
                Id = _nextVacationId++,
            };

            _vacations.Add(vacation.Id, vacation);
            for (int i = 0; i < duration; i++)
            {
                if(_vacationCalendar.TryGetValue(begin.AddDays(i), out List<int> vacationsList))
                {
                    vacationsList.Add(vacation.Id);
                } else
                {
                    vacationsList = new List<int> { vacation.Id };
                    _vacationCalendar.Add(begin.AddDays(i), vacationsList);
                }
            }
            return true;
        }

        /// <summary>
        /// Adds a vacation of a given length starting from an random date in a given year.
        /// </summary>
        /// <param name="employee">The employee for whom the vacation is being created.</param>
        /// <param name="year">Target year.</param>
        /// <param name="duration">Vacation duration.</param>
        /// <param name="attempts">The number of attempts to add a vacation.</param>
        /// <returns>Returns false if it was not possible to create a vacation in the specified 
        /// number of attempts. </returns>
        public bool AddVacationToRandomDate(Employee employee, int year, int duration, int attempts = 10)
        {
            int daysRate = DateTime.IsLeapYear(year) ? 366 : 365;
            DateTime vacationBegin = new(year, 1, 1);
            for (int i = 0; i < attempts; i++)
            {
                int randomDayInYear = _random.Next(daysRate - duration);
                vacationBegin = vacationBegin.AddDays(randomDayInYear);
                if (AddVacation(employee, vacationBegin, duration))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Find All vacation of given employee in storage.
        /// </summary>
        /// <param name="employeeId">Id of employee.</param>
        /// <returns>Collection of employee vacation's.</returns>
        public IEnumerable<Vacation> GetAllVacationsByEmployeeId(int employeeId)
        {
            var employeeVacations = _vacations.Select(kvp => kvp.Value)
                .Where(v => v.Employee.Id == employeeId);
            return employeeVacations;
        }

        /// <summary>
        /// Remove vacation from calendar and from storage.
        /// </summary>
        /// <param name="vacationId">Id of vacation.</param>
        public void RemoveVacation(int vacationId)
        {
            if (_vacations.TryGetValue(vacationId, out Vacation vacation))
            {
                DateTime date = vacation.Begin;

                while(date <= vacation.End)
                {
                    _vacationCalendar.Remove(date);
                    date.AddDays(1);
                }
                _vacations.Remove(vacationId);
            }
        }

        /// <summary>
        /// Find All vacations in given period.
        /// </summary>
        /// <param name="begin">Search begin</param>
        /// <param name="duration">Search duration</param>
        /// <returns>Collection of vacation's</returns>
        public IEnumerable<Vacation> GetVacations(DateTime begin, int duration)
        {
            List<int> vacationsId = new();
            for (int i = 0; i < duration; i++)
            {
                if (_vacationCalendar.TryGetValue(begin.AddDays(i), out List<int> vacationsList))
                {
                    vacationsId = vacationsId.Union(vacationsList).ToList();
                }
            }
            var vacations = vacationsId.Select(id => _vacations[id]);
            return vacations;
        }

        /// <summary>
        /// Find ALL vacations that have NO intersections whith other
        /// </summary>
        /// <returns>Collection of vacation's</returns>
        public IEnumerable<Vacation> GetAllNotIntersectingVacations()
        {
            var vacationsWhithNotIntersectingDays = _vacationCalendar.Select(kvp => kvp.Value)
                .Where(list => list.Count == 1)
                .SelectMany(list => list)
                .Distinct(); 
            
            var vacationsWhithInstrictedDays = _vacationCalendar.Select(kvp => kvp.Value)
                .Where(list => list.Count > 1)
                .SelectMany(list => list)
                .Distinct();

            var notIntersectingVacations = vacationsWhithNotIntersectingDays.Except(vacationsWhithInstrictedDays)
                .Select(id => _vacations[id]);

            return notIntersectingVacations;
        }
    }
}
