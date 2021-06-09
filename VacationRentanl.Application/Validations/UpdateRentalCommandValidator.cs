using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Application.Commands;
using VacationRental.Domain.AggregatesModel;

namespace VacationRental.Application.Validations
{
    public class UpdateRentalCommandValidator : AbstractValidator<UpdateRentalBindingModel>
    {
        public UpdateRentalCommandValidator(IDictionary<int, Rental> _rental)
        {
            RuleFor(command => command.Units)
                .GreaterThan(0)
                .WithMessage("Units must be greather than zero!");

            RuleFor(command => command.PreparationTimeInDays)
                .GreaterThan(0)
                .WithMessage("Days of preparation must be greather than zero!");

            RuleFor(command => command.Id)
             .NotEmpty()
             .Must((model, cancellation) =>
             {
                 if (model.Id == 0)
                     return false;

                 var hasValue = _rental.ContainsKey(model.Id);

                 return hasValue;
             }).WithMessage("Rental not found");
        }
    }
}