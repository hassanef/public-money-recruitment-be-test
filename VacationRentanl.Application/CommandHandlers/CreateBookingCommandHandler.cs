using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Commands;
using VacationRental.Domain.AggregatesModel;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Application.CommandHandlers
{
    public  class CreateBookingCommandHandler : IRequestHandler<BookingBindingModel, Booking>
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        public CreateBookingCommandHandler(IDictionary<int, Rental> rentals,
                                           IDictionary<int, Booking> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public Task<Booking> Handle(BookingBindingModel request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new VacationRentalException("request BookingBindingModel is null!");

            var rental = _rentals[request.RentalId];
            var bookings = _bookings.Where(x => x.Value.RentalId == request.RentalId)
                                    .Select(x => x.Value)
                                    .ToList();

            var booking = Booking.CreateBooking(_bookings.Keys.Count, request.RentalId, request.Start, request.Nights, rental.Units, rental.PreparationTimeInDays, bookings);

            _bookings.Add(booking.Id, booking);
            
            return Task.FromResult(booking);
        }
    }
}