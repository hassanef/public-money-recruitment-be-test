using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Application.Commands;
using VacationRental.Application.ViewModels;
using VacationRental.Domain.AggregatesModel;
using VacationRental.Domain.Exceptions;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, Rental> _rentals;
        private readonly IMediator _mediator;

        public RentalsController(IDictionary<int, Rental> rentals, IMediator mediator)
        {
            _rentals = rentals;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            if (rentalId <= 0)
                throw new VacationRentalException("rentalId must be positive");

            if (!_rentals.ContainsKey(rentalId))
                throw new VacationRentalException("Rental not found");

            var rental = _rentals.Where(x => x.Value.Id == rentalId)
                                 .Select(x => new RentalViewModel()
                                 {
                                     PreparationTimeInDays = x.Value.PreparationTimeInDays,
                                     Units = x.Value.Units
                                 }).SingleOrDefault();

            return rental;
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var result = await _mediator.Send(model);

            var key = new ResourceIdViewModel { Id = result.Id };

            return key;
        }
        [HttpPut]
        public async Task<ResourceIdViewModel> Put(UpdateRentalBindingModel model)
        {
            var result = await _mediator.Send(model);

            var key = new ResourceIdViewModel { Id = result.Id };

            return key;
        }
    }
}
