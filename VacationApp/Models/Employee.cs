using System;
using System.ComponentModel.DataAnnotations;

namespace VacationApp.Models
{
    /// <summary>
    /// Gender is required for employee name generator.
    /// </summary>
    public enum Gender
    {
        Male,
        Female
    }
    /// <summary>
    /// Simple employee model
    /// </summary>
    public class Employee
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя работника")]
        public string Name{ get; set; }

        public string SecondName { get; set; }

        [Required(ErrorMessage = "Не указана фамилия работника")]
        public string Surname { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Недопустимый возраст")]
        public int Age{ get; set; }

        [Required(ErrorMessage = "Не указан пол")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Не указана должность")]
        public String Position { get; set; }

        public override string ToString()
        {
            return String.Format("Сотрудник: {0} {1} {2} Должность: {3}", Name, SecondName, Surname, Position);
        }
    }

}
