using System;
using System.Collections.Generic;
using VacationApp.Serviсes;
using VacationApp.Models; 

namespace VacationApp
{
    class Program
    {


        static void Main(string[] args)
        {



            RandomEmployeeGenerator reg = new();
            VacationService vs = new();
            List<Employee> employees = new();
            int currentYear = DateTime.Today.Year;

            for (int i = 0; i < 100; i++)
            {
                employees.Add(reg.GetNewEmployee());
            }

            foreach (var employee in employees)
            {
                vs.AddVacationToRandomDate(employee, currentYear, 14);
                vs.AddVacationToRandomDate(employee, currentYear, 7);
                vs.AddVacationToRandomDate(employee, currentYear, 7);
            }
            var testEmployee = reg.GetNewEmployee();
            Console.WriteLine("{0} {1} {2} собирается в отпуск.\r\n" +
                "Введите дату начала отпуска в формате \"dd / MM / yyyy\"", 
                testEmployee.Name, testEmployee.SecondName, testEmployee.Surname);
            var input = Console.ReadLine();
            if (input[0] == 'q')
                Environment.Exit(0);
            var beginDate = DateTime.Parse(input);

            Console.WriteLine("Отпуска без пересечения:");
            foreach (var vacation in vs.GetAllNotIntersectingVacations())
            {
                Console.WriteLine(vacation.ToString());
            }

        }
    }
}
