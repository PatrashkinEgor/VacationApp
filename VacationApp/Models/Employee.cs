using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VacationApp.Models
{

    public enum Position
    {
        Менеджер,
        Аудитор,
        Мастер,
        Инженер,
        Лаборант,
        Диспетчер,
        Бухгалтер,
        Переводчик,
        Кассир,
        Редактор
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class Employee
    {

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
        public Position Position { get; set; }

        public override string ToString()
        {
            return "Работник: " + Name + "\t" + SecondName + "\t" + Surname + ".\t Должность: " + Position;
        }
    }

}
