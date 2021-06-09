using System;
using System.Collections.Generic;
using System.Text;

namespace VacationRental.Application.ViewModels
{
    public class BookingViewModel
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
