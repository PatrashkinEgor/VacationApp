using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationApp.Models
{
    public class Vacation
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1} - {2} {3} {4} - {5} ", 
                Begin.ToString("dd/MM/yyyy"), End.ToString("dd/MM/yyyy"), 
                Employee.Name, Employee.SecondName, Employee.Surname, Employee.Age);
        }
    }
}
