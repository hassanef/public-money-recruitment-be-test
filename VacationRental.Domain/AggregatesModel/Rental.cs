using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.DomainDrivenDesign.Domain.SeedWork;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Domain.AggregatesModel
{
    public class Rental : Entity, IAggregateRoot
    {
        public int Units { get; private set; }
        public int PreparationTimeInDays { get; private set; }

        public Rental(int lastId, int units, int preparationTimeInDays)
        {
            //I think Id is not issue in this senario because when using database it generated automatically
            Id = lastId + 1;
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public void SetUnits(int units, int oldUnits, int preparationTimeInDays, List<Booking> bookings)
        {
            if (units < oldUnits && (bookings != null && bookings.Any()))
            {
                foreach (var booking in bookings)
                {
                    var count = BookingsCount(booking, bookings, preparationTimeInDays);
                 
                    if (count > units)
                    {
                        throw new VacationRentalException("Request Failed");
                    }
                }
            }

            Units = units;
        }

        public void SetPreparationTimeInDays(int preparationTimeInDays, int units, int oldPreparationTimeInDays, List<Booking> bookings)
        {

            if (preparationTimeInDays > oldPreparationTimeInDays && (bookings != null && bookings.Any()))
            {
                foreach (var booking in bookings)
                {
                    var count = BookingsCount(booking, bookings, preparationTimeInDays);

                    if (count >= units)
                    {
                        throw new VacationRentalException("Request Failed");
                    }
                }
            }

            PreparationTimeInDays = preparationTimeInDays;
        }
        private int BookingsCount(Booking booking, List<Booking> bookings, int preparationTimeInDays)
        {
            var count = bookings.Count(x => x.Id != booking.Id && (x.Start <= booking.Start.Date && x.Start.AddDays(x.Nights).AddDays(preparationTimeInDays) > booking.Start.Date) ||
                                            (x.Start < booking.Start.AddDays(booking.Nights).AddDays(preparationTimeInDays) && x.Start.AddDays(x.Nights).AddDays(preparationTimeInDays) >= booking.Start.AddDays(booking.Nights).AddDays(preparationTimeInDays)) ||
                                            (x.Start > booking.Start && x.Start.AddDays(x.Nights).AddDays(preparationTimeInDays) < booking.Start.AddDays(booking.Nights).AddDays(preparationTimeInDays)));

            return count;
        }

    }
}
