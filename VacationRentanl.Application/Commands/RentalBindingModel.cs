using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.AggregatesModel;

namespace VacationRental.Application.Commands
{
    public class RentalBindingModel : IRequest<Rental>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

    }
    public class UpdateRentalBindingModel : RentalBindingModel
    {
        public int Id { get; set; }
    }
}
