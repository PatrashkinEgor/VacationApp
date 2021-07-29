using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationApp.Models
{
    public class Vacation
    {
        public Employee Employee{ get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}
