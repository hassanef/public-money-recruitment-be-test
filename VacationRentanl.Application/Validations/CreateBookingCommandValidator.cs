using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Application.Commands;
using VacationRental.Domain.AggregatesModel;

namespace VacationRental.Application.Validations
{
    public class CreateBookingCommandValidator : AbstractValidator<BookingBindingModel>
    {
        public CreateBookingCommandValidator(IDictionary<int, Rental> _rental)
        {
            RuleFor(command => command.Nights)
                .GreaterThan(0)
                .WithMessage("Nigts must be positive");

            RuleFor(command => command.Start)
                .NotEmpty();

            RuleFor(command => command.RentalId)
             .NotEmpty()
             .Must((model, cancellation) =>
             {
                 if (model.RentalId <= 0)
                     return false;

                 var hasValue = _rental.ContainsKey(model.RentalId);

                 return hasValue;
             }).WithMessage("Rental not found");
        }
    }
}