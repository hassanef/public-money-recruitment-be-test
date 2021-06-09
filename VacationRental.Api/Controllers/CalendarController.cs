using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Application.Queries;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarQuery _calendarQuery;

        public CalendarController(ICalendarQuery calendarQuery)
        {
            _calendarQuery = calendarQuery;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights <= 0)
                throw new VacationRentalException("Nights must be positive");

            var calendar = _calendarQuery.Get(rentalId, start, nights);

            return calendar;
        }
    }
}
