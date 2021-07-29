using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationApp.Models
{
    class VacationCalendar
    {
        private Hashtable Days;
        public VacationCalendar()
        {
            Days = new Hashtable();
        }

        public void AddVacation(Vacation vacation)
        {
            if (Days.Contains(vacation.Begin))
            {
                 VacationDay  vd = Days[vacation.Begin] as VacationDay;
            }
        }
    }
}
