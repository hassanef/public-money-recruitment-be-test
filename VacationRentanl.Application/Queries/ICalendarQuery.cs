using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.AggregatesModel;

namespace VacationRental.Application.Queries
{
    public interface ICalendarQuery
    {
        CalendarViewModel Get(int rentalId, DateTime start, int nights);
    }
}
