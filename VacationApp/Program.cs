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
            for (int i = 0; i < 100; i++)
            {
                Employee employee = reg.GetNewEmployee();
                Console.WriteLine(employee.ToString());
            }

        }
    }
}
