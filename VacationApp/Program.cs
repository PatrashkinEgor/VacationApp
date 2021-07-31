using System;
using System.Collections.Generic;
using VacationApp.Serviсes;
using VacationApp.Models;
using System.Linq;
using System.IO;

namespace VacationApp
{
    class Program
    {
        const string FileName = "out.txt";
        const int NumberOfEmployees = 10000;
        const int MaxVacationDuration = 14;
        const int StandartVacationDuration = 7;
        const int YoungEmploeeBorder = 30;
        const int OldEmploeeBorder = 50;

        static void Main(string[] args)
        {
            RandomEmployeeGenerator reg = null;
            try
            {
                 reg = new();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Не удалось найти файл ресурсов для генератора сотрудников..\n"+
                    "Работа программы завершена. Нажмите Enter.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            
            VacationService vacationService = new();
            List<Employee> employees = new();
            int currentYear = DateTime.Today.Year;

            for (int i = 0; i < NumberOfEmployees; i++)
            {
                employees.Add(reg.GetNewEmployee());
            }
            Console.WriteLine("{0} сотрудников создано", NumberOfEmployees);

            foreach (var employee in employees)
            {
                vacationService.AddVacationToRandomDate(employee, currentYear, MaxVacationDuration);
                vacationService.AddVacationToRandomDate(employee, currentYear, StandartVacationDuration);
                vacationService.AddVacationToRandomDate(employee, currentYear, StandartVacationDuration);
            }
            var testEmployee = reg.GetNewEmployee();
            Console.WriteLine("{0} {1} {2} собирается в отпуск.",
                testEmployee.Name, testEmployee.SecondName, testEmployee.Surname);

            DateTime beginDate = new();
            int duration = 0;
            while (true)
            {
                try
                {
                    GetNewVacationDatesOrExit(ref beginDate, ref duration);
                    break;
                }
                catch (VacationDatesFormatException e)
                {
                    Console.WriteLine("Неверный формат ввода. " + e.Message);
                }
            }
            vacationService.AddVacation(testEmployee, beginDate, duration);
            Console.WriteLine("Отпуск успешно создан!");

            var vacationsOnTestPeriod = vacationService.GetVacations(beginDate, duration)
                .Where(v => v.Employee.Id != testEmployee.Id)
                .OrderBy(v => v.Employee.Age);
            var notIntersectingVacations = vacationService.GetAllNotIntersectingVacations();
            WriteVacationsStatToFile(@"..\" + FileName, vacationsOnTestPeriod, notIntersectingVacations);
            Console.WriteLine("Статистика записана в файл {0}.", FileName);
            Console.WriteLine("Работа программы завершена. Нажмите Enter.");
            Console.ReadLine();
        }





        static void WriteVacationsStatToFile(string path, IEnumerable<Vacation> vacationsOnTestPeriod, IEnumerable<Vacation> notIntersectingVacations)
        {
            using (StreamWriter sw = new(path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine("Пересечение отпуска с сотрудниками, моложе {0}:", YoungEmploeeBorder);
                foreach (var vacation in vacationsOnTestPeriod
                    .Where(v => v.Employee.Age < YoungEmploeeBorder))
                {
                    sw.WriteLine(vacation.ToString());
                };
                sw.WriteLine();

                sw.WriteLine("Пересечение отпуска с сотрудниками, старше {0} моложе {1}:"
                    , YoungEmploeeBorder, OldEmploeeBorder);
                foreach (var vacation in vacationsOnTestPeriod
                    .Where(v => v.Employee.Age >= YoungEmploeeBorder && v.Employee.Age < OldEmploeeBorder))
                {
                    sw.WriteLine(vacation.ToString());
                };
                sw.WriteLine();

                sw.WriteLine("Пересечение отпуска с сотрудниками, старше {0}:", OldEmploeeBorder);
                foreach (var vacation in vacationsOnTestPeriod
                    .Where(v => v.Employee.Age >= OldEmploeeBorder))
                {
                    sw.WriteLine(vacation.ToString());
                };
                sw.WriteLine();

                sw.WriteLine("Отпуска без пересечения:");
                foreach (var vacation in notIntersectingVacations)
                {
                    sw.WriteLine(vacation.ToString());
                }
            }
        }

        static void GetNewVacationDatesOrExit(ref DateTime beginDate, ref int duration)
        {
            Console.WriteLine("Введите даты отпуска в формате \"dd / MM / yyyy - dd / MM / yyyy\".\n" +
                "Нажмите q для завершения работы.");

            var input = Console.ReadLine().Replace(" ", "");
            if (input[0] == 'q')
                Environment.Exit(0);

            var dates = input.Split("-");
            if (dates.Length != 2)
            {
                throw new VacationDatesFormatException("Не найден разделитель.");
            }

            DateTime endDate;
            try
            {
                beginDate = DateTime.Parse(dates[0]);
            }
            catch (FormatException)
            {
                throw new VacationDatesFormatException("Не удалось распознать дату начала отпуска.");
            }

            try
            {
                endDate = DateTime.Parse(dates[1]);
            }
            catch (FormatException)
            {
                throw new VacationDatesFormatException("Не удалось распознать дату конца отпуска.");
            }

            duration = endDate.Subtract(beginDate).Days;
            if (duration <= 0)
            {
                throw new VacationDatesFormatException("Конец отпуска раньше, либо равен его началу.");
            }
            if (duration > MaxVacationDuration)
            {
                throw new VacationDatesFormatException(String.Format("Отпуск долше {0} дней невозможен."
                    , MaxVacationDuration));
            }
        }
    }
}
