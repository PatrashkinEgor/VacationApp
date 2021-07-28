using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationApp.Models;


namespace VacationApp.Serviсes
{
    interface IEmployeeFactory
    {
        public Employee GetNewEmployee();
    }
}
