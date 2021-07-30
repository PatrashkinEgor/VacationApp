using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationApp.Models
{
    class VacationService
    {
        private readonly Dictionary<DateTime, List<int>> _vacationCalendar = new ();
        private readonly Dictionary<int, Vacation> _vacations = new ();
        private int _nextVacationId = 0;


        public void AddVacation(Employee employee, DateTime begin, int duration)
        {
            if (begin.CompareTo(DateTime.Today) <= 0)
                throw new ArgumentOutOfRangeException(nameof(begin), "The selected date is not available.");

            var intersections = GetIntersectingVacations(begin, duration);
            foreach (var inter in intersections)
            {
                if (inter.Employee.Id == employee.Id)
                    throw new ArgumentOutOfRangeException(nameof(begin), "Employee already have vacation at this period!");
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
        }

        public IEnumerable<Vacation> GetIntersectingVacations(DateTime begin, int duration)
        {
            List<int> intersectingVacationsId = new();
            for (int i = 0; i < duration; i++)
            {
                if (_vacationCalendar.TryGetValue(begin.AddDays(i), out List<int> vacationsList))
                {
                    intersectingVacationsId = intersectingVacationsId.Union(vacationsList).ToList();
                }
            }
            var intersectingVacations = intersectingVacationsId.Select(id => _vacations[id]);
            return intersectingVacations;
        }

        public IEnumerable<Vacation> GetAllNotIntersectingVacations()
        {
            var vacationsWhithNotInstrictedDays = _vacationCalendar.Select(kvp => kvp.Value)
                .Where(list => list.Count == 1)
                .SelectMany(list => list)
                .Distinct(); 
            
            var vacationsWhithInstrictedDays = _vacationCalendar.Select(kvp => kvp.Value)
                .Where(list => list.Count > 1)
                .SelectMany(list => list)
                .Distinct();

            var notInstrictedVacations = vacationsWhithNotInstrictedDays.Except(vacationsWhithInstrictedDays)
                .Select(id => _vacations[id]);

            return notInstrictedVacations;
        }
    }
}
