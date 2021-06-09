using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.AggregatesModel;

namespace VacationRental.Application.Commands
{
    public class BookingBindingModel : IRequest<Booking>
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }
    }
}
