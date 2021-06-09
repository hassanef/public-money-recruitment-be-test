using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.DomainDrivenDesign.Domain.SeedWork;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Domain.AggregatesModel
{
    public class Booking : Entity, IAggregateRoot
    {
        public int RentalId { get; private set; }
        public DateTime Start { get; private set; }
        public int Nights { get; private set; }

        private Booking(int lastId, int rentalId, DateTime start, int nights)
        {
            Id = lastId + 1;
            RentalId = rentalId;
            Start = start;
            Nights = nights;
        }
     
        public static Booking CreateBooking(int lastId, int rentalId, DateTime start, int nights, int units, int preparationTimeInDays, List<Booking> bookings)
        {
            if (bookings != null && bookings.Any())
            {
                var count = bookings.Count(x => (x.Start <= start.Date && x.Start.AddDays(x.Nights).AddDays(preparationTimeInDays) > start.Date) ||
                                                (x.Start < start.AddDays(nights).AddDays(preparationTimeInDays) && x.Start.AddDays(x.Nights).AddDays(preparationTimeInDays) >= start.AddDays(nights).AddDays(preparationTimeInDays)) ||
                                                (x.Start > start && x.Start.AddDays(x.Nights).AddDays(preparationTimeInDays) < start.AddDays(nights).AddDays(preparationTimeInDays)));


                if (count >= units)
                    throw new VacationRentalException("Not available");
            }

            return new Booking(lastId, rentalId, start, nights);
        }
     }
}
