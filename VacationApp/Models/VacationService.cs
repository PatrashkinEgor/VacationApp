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
        private readonly Random _random = new();
        private int _nextVacationId = 0;


        public bool AddVacation(Employee employee, DateTime begin, int duration)
        {
            /*
            var intersections = GetVacations(begin, duration);
            foreach (var inter in intersections)
            {
                if (inter.Employee.Id == employee.Id)
                    return false;
            }
            */

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

        public IEnumerable<Vacation> GetAllVacationsByEmployeeId(int employeeId)
        {
            var employeeVacations = _vacations.Select(kvp => kvp.Value)
                .Where(v => v.Employee.Id == employeeId);
            return employeeVacations;
        }

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
