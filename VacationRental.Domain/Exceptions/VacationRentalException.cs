using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Domain.Exceptions
{
    public class VacationRentalException : Exception
    {
        public VacationRentalException()
        { }

        public VacationRentalException(string message)
            : base(message)
        { }

        public VacationRentalException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}


