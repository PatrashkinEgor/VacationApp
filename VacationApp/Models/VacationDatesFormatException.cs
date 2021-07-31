using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationApp.Models
{
    class VacationDatesFormatException : FormatException
    {
        public VacationDatesFormatException(string message)
            : base(message)
        { }
    }
}
