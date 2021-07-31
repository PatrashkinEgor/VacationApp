using System;
using System.ComponentModel.DataAnnotations;

namespace VacationApp.Models
{
    /// <summary>
    /// Simple vacation model
    /// </summary>
    public class Vacation
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Employee Employee { get; set; }
        [Required]
        public DateTime Begin { get; set; }
        [Required]
        public DateTime End { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1} - {2} {3} {4} - {5} ", 
                Begin.ToString("dd/MM/yyyy"), End.ToString("dd/MM/yyyy"), 
                Employee.Name, Employee.SecondName, Employee.Surname, Employee.Age);
        }
    }
}
