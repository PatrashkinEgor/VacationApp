using System;

namespace VacationApp.Models
{
    /// <summary>
    /// This exception is thrown when the format of the string with vacation dates is violated.
    /// </summary>
    class VacationDatesFormatException : FormatException
    {
        public VacationDatesFormatException(string message)
            : base(message)
        { }
    }
}
