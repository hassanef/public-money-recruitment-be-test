using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Commands;
using VacationRental.Domain.AggregatesModel;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Application.CommandHandlers
{
    public class CreateRentalCommandHandler : IRequestHandler<RentalBindingModel, Rental>
    {
        private readonly IDictionary<int, Rental> _rentals;
        public CreateRentalCommandHandler(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }
        public Task<Rental> Handle(RentalBindingModel request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new VacationRentalException("request RentalBindingModel is null!");

            var rental = new Rental(_rentals.Keys.Count, request.Units, request.PreparationTimeInDays);

             _rentals.Add(rental.Id, rental);

            return Task.FromResult(rental);
        }
    }
}