using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.AggregatesModel;
using VacationRental.Domain.Exceptions;
using Xunit;

namespace VacationRental.Domain.UnitTest
{
    public class RentalTests
    {
        private Rental _rental;
        private List<Booking> _bookings = new List<Booking>();

        public RentalTests()
        {
            _rental = new Rental(0, 2, 1);
        }

        [Fact]
        public void GivenNothing_WhenSetUnits_ThenShouldNotThrowException()
        {
            _bookings = new List<Booking>();
            AddFakeBookings();
            _rental.SetUnits(3, 2, 1, _bookings);

            // Assert
            Assert.True(true);
        }
        [Fact]
        public void GivenNothing_WhenSetUnits_ThenShouldThrowVacationException()
        {
            _bookings = new List<Booking>();
            AddFakeBookings();

            // Assert
            Assert.Throws<VacationRentalException>(() => _rental.SetUnits(1, 2, 1, _bookings));
        }
        [Fact]
        public void GivenNothing_WhenSetPreparationTimeInDays_ThenShouldNotThrowException()
        {
            _bookings = new List<Booking>();
            AddFakeBookings();

            _rental.SetPreparationTimeInDays(0, 2, 1, _bookings);

            // Assert
            Assert.True(true);
        }
        [Fact]
        public void GivenNothing_WhenSetPreparationTimeInDays_ThenShouldThrowVacationException()
        {
            _bookings = new List<Booking>();
            AddFakeBookings();


            // Assert
            Assert.Throws<VacationRentalException>(() => _rental.SetPreparationTimeInDays(2, 2, 1, _bookings));
        }
        private void AddFakeBookings()
        {
            var book1 = Booking.CreateBooking(0, _rental.Id, new DateTime(2020, 01, 02), 1, _rental.Units, _rental.PreparationTimeInDays, _bookings);
            _bookings.Add(book1);

            var book2 = Booking.CreateBooking(1, _rental.Id, new DateTime(2020, 01, 03), 1, _rental.Units, _rental.PreparationTimeInDays, _bookings);
            _bookings.Add(book2);
        }
    
    }

}
