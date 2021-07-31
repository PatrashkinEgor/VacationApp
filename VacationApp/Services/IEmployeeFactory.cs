using VacationApp.Models;


namespace VacationApp.Services
{
    /// <summary>
    /// Simple interface to reduce coupling.
    /// Not useful in this project. May be useful when extending the application.
    /// </summary>
    interface IEmployeeFactory
    {
        public Employee GetNewEmployee();
    }
}
