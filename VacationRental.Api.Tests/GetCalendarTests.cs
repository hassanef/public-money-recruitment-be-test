using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Application.Commands;
using VacationRental.Application.ViewModels;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;
        private int _rentalId;
        private CalendarViewModel _getCalendarResult;
        private int _bookId1;
        private int _bookId2;

        public GetCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
            When().GetAwaiter().GetResult();
        }

        private async Task When()
        {
            _rentalId = await When_PostRental();
            (_bookId1, _bookId2) = await When_PostBooking();
            _getCalendarResult = await When_GetCalendar();
        }

        private async Task<CalendarViewModel> When_GetCalendar()
        {
            CalendarViewModel getCalendarResult;
            using (var getCalendarResponse =
                await _client.GetAsync($"/api/v1/calendar?rentalId={_rentalId}&start=2000-01-01&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);
                getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
            }

            return getCalendarResult;
        }

        private async Task<int> When_PostRental()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            return postRentalResult.Id;
        }

        private async Task<(int bookId1, int bookId2)> When_PostBooking()
        {
            var postBooking1Request = new BookingBindingModel
            {
                RentalId = _rentalId,
                Nights = 2,
                Start = new DateTime(2000, 01, 02)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = _rentalId,
                Nights = 2,
                Start = new DateTime(2000, 01, 03)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            return (postBooking1Result.Id, postBooking2Result.Id);
        }


        [Fact]
        public void ThenCalendarDatesShouldEqualFive()
        {
            Assert.Equal(_rentalId, _getCalendarResult.RentalId);
            Assert.Equal(5, _getCalendarResult.Dates.Count);
        }

        [Fact]
        public void ThenDates_OfFirstCalendarBooking_ShouldBeEmptyBooking()
        {
            Assert.Equal(new DateTime(2000, 01, 01), _getCalendarResult.Dates[0].Date);
            Assert.Empty(_getCalendarResult.Dates[0].Bookings);
        }

        [Fact]
        public void ThenDates_OfSecondCalendarBooking_HasOneBooking()
        {
            Assert.Equal(new DateTime(2000, 01, 02), _getCalendarResult.Dates[1].Date);
            Assert.Single(_getCalendarResult.Dates[1].Bookings);
            Assert.Contains(_getCalendarResult.Dates[1].Bookings, x => x.Id == _bookId1);
        }

        [Fact]
        public void ThenDates_OfThirdCalendarBooking_HasTwoBookings()
        {
            Assert.Equal(new DateTime(2000, 01, 03), _getCalendarResult.Dates[2].Date);
            Assert.Equal(2, _getCalendarResult.Dates[2].Bookings.Count);
            Assert.Contains(_getCalendarResult.Dates[2].Bookings, x => x.Id == _bookId1);
            Assert.Contains(_getCalendarResult.Dates[2].Bookings, x => x.Id == _bookId2);
        }

        [Fact]
        public void ThenDates_OfForthCalendarBooking_HasOneBooking()
        {
            Assert.Equal(new DateTime(2000, 01, 04), _getCalendarResult.Dates[3].Date);
            Assert.Single(_getCalendarResult.Dates[3].Bookings);
            Assert.Contains(_getCalendarResult.Dates[3].Bookings, x => x.Id == _bookId2);
        }

        [Fact]
        public void ThenDates_OfFifthCalendarBooking_ShouldBeEmptyBooking()
        {
            Assert.Equal(new DateTime(2000, 01, 05), _getCalendarResult.Dates[4].Date);
            Assert.Empty(_getCalendarResult.Dates[4].Bookings);
        }
    }
}
