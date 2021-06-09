using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Application.Commands;

namespace VacationRental.Application.Validations
{
    public class CreateRentalCommandValidator : AbstractValidator<RentalBindingModel>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(command => command.Units)
                .GreaterThan(0)
                .WithMessage("Units must be greather than zero!");
            
            RuleFor(command => command.PreparationTimeInDays)
                .GreaterThan(0)
                .WithMessage("Days of preparation must be greather than zero!");

        }
    }
}