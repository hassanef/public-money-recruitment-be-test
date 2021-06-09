using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.AggregatesModel;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Application.Queries
{
    public class CalendarQuery : ICalendarQuery
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        public CalendarQuery(IDictionary<int, Rental> rentals, IDictionary<int, Booking> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (!_rentals.ContainsKey(rentalId))
                throw new VacationRentalException("Rental not found");

            var units = _rentals[rentalId].Units;
            int preparationTimeInDays = _rentals[rentalId].PreparationTimeInDays;
            int currentUnit = 0;
            var currentPreparationUnit = 0;
            var bookings = _bookings.Values.Where(x => x.RentalId == rentalId).ToList();

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<UnitViewModel>()
                };

                for (var j = 0; j < bookings.Count; j++)
                {
                    if (bookings[j].Start <= date.Date && bookings[j].Start.AddDays(bookings[j].Nights) > date.Date)
                    {
                        var currentBookingUnit = GetBookingUnit(result, date, bookings[j], currentUnit, units);

                        date.Bookings.Add(new CalendarBookingViewModel { Id = bookings[j].Id, Unit = currentBookingUnit });

                    }
                    if (bookings[j].Start <= date.Date && bookings[j].Start.AddDays(bookings[j].Nights) <= date.Date &&
                       bookings[j].Start.AddDays(bookings[j].Nights).AddDays(preparationTimeInDays) > date.Date)
                    {
                        int? unit = null;

                        unit = result.Dates?.SelectMany(x => x.Bookings)
                                           .FirstOrDefault(x => x.Id == bookings[j].Id)?.Unit;

                        if (unit == null)
                        {
                            currentPreparationUnit = units;
                        }
                        else
                        {
                            currentPreparationUnit = unit.Value;
                        }


                        date.PreparationTimes.Add(new UnitViewModel { Unit = currentPreparationUnit });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }

        private int GetBookingUnit(CalendarViewModel result, CalendarDateViewModel date, Booking booking, int currentUnit, int units)
        {
            int? unit = null;

            if (result.Dates.SelectMany(x => x.Bookings).Any())
            {
                unit = result.Dates.SelectMany(x => x.Bookings)
                                   .FirstOrDefault(x => x.Id == booking.Id)?.Unit;

                if (unit != null)
                {
                    currentUnit = unit.Value;
                }
                else
                {
                    unit = result.Dates.SelectMany(x => x.Bookings).Last().Unit;


                    if (unit == units)
                    {
                        currentUnit = 1;
                    }
                    else
                    {
                        currentUnit = unit.Value + 1;
                    }
                }
            }
            else if (date.Bookings.Any())
            {
                var lastBooking = date.Bookings.Last();

                if (lastBooking.Id == booking.Id)
                {
                    currentUnit = lastBooking.Unit;
                }
                else
                {
                    if (date.Bookings.Last().Unit == units)
                    {
                        currentUnit = 1;
                    }
                    else
                    {
                        currentUnit = date.Bookings.Last().Unit + 1;
                    }
                }
            }
            else
            {
                if (booking.Start <= date.Date)
                {
                    currentUnit = 1;
                }
            }

            return currentUnit;
        }
    }
}
