using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.AggregatesModel;
using VacationRental.Domain.Exceptions;
using Xunit;

namespace VacationRentanl.Domain.UnitTests
{
    public class BookingTests
    {
        private Rental _rental;
        private List<Booking> _bookings = null;

        public BookingTests()
        {
            _rental = new Rental(0, 2, 1);
        }

        [Fact]
        public void GivenABooking_WhenAddBooking_ThenBookingsShouldContainTheBooking()
        {
            _bookings = new List<Booking>();
            var book = Booking.CreateBooking(0, _rental.Id, new DateTime(2020, 01, 02), 1, _rental.Units, _rental.PreparationTimeInDays, _bookings);

            // Assert
            Assert.NotNull(book);
            Assert.Equal(1, book.Id);
        }

        [Fact]
        public void GivenNothing_WhenAddBooking_ThenShouldThrowVacationException()
        {
            _bookings = new List<Booking>();
            AddFakeBookings();
        
            // Assert
            Assert.Throws<VacationRentalException>(() => _bookings.Add(Booking.CreateBooking(2, _rental.Id, new DateTime(2020, 01, 02), 1, _rental.Units, _rental.PreparationTimeInDays, _bookings)));
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
