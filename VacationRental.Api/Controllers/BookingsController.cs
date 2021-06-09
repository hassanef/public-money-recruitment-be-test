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
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, Booking> _bookings;
        private readonly IMediator _mediator;

        public BookingsController(IDictionary<int, Booking> bookings,
                                  IMediator mediator)
        {
            _bookings = bookings;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            if (bookingId <= 0)
                throw new VacationRentalException("bookingId must be positive");

            if (!_bookings.ContainsKey(bookingId))
                throw new VacationRentalException("Booking not found");

            var booking = _bookings.Where(x => x.Value.Id == bookingId)
                                .Select(x => new BookingViewModel()
                                {
                                    Nights = x.Value.Nights,
                                    RentalId = x.Value.RentalId,
                                    Start = x.Value.Start
                                }).SingleOrDefault();

            return booking;
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            var result = await _mediator.Send(model);

            var key = new ResourceIdViewModel { Id = result.Id };

            return key;
        }
    }
}
