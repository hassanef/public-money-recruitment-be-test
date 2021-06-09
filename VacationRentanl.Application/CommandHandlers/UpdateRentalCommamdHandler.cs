using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Commands;
using VacationRental.Domain.AggregatesModel;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Application.CommandHandlers
{
    public class UpdateRentalCommamdHandler : IRequestHandler<UpdateRentalBindingModel, Rental>
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IDictionary<int, Booking> _bookings;
        public UpdateRentalCommamdHandler(IDictionary<int, Rental> rentals,
                                          IDictionary<int, Booking> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public Task<Rental> Handle(UpdateRentalBindingModel request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new VacationRentalException("request UpdateRentalBindingModel is null!");

            var rental = _rentals[request.Id];
            var bookings = _bookings.Where(x => x.Value.RentalId == request.Id)
                                    .Select(x => x.Value)
                                    .ToList();

            rental.SetUnits(request.Units, rental.Units, request.PreparationTimeInDays, bookings);
            rental.SetPreparationTimeInDays(request.PreparationTimeInDays, request.Units, rental.PreparationTimeInDays, bookings);

            _rentals[request.Id] = rental;

            return Task.FromResult(rental);
        }
    }
}