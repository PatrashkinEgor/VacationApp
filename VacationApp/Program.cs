using System;
using VacationApp.Serviсes;
using VacationApp.Models; 

namespace VacationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RandomEmployeeGenerator reg = new();
            Employee employee = reg.GetNewEmployee();
            Console.WriteLine(employee.ToString());
            VacationService vs = new();
            DateTime begin = DateTime.Today.AddDays(1);
            vs.AddVacation(employee, begin, 7);
            employee = reg.GetNewEmployee();
            vs.AddVacation(employee, begin.AddDays(3), 7);
            employee = reg.GetNewEmployee();
            vs.AddVacation(employee, begin.AddDays(30), 7);



            Console.WriteLine("Отпуска без пересечения:");
            foreach (var vacation in vs.GetAllNotIntersectingVacations())
            {
                Console.WriteLine(vacation.ToString());
            }

        }
    }
}
