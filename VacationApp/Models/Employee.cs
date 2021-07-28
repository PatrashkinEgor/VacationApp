using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationApp.Models
{

    enum Gender
    {
        Male,
        Female
    }

    class Employee
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }
        public int Age{ get; set; }
        public Gender Gender { get; set; }
    }
}
