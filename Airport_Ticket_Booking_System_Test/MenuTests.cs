// File: Airport_Ticket_Booking_System_Test/MenuTests.cs
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Moq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Airport_Ticket_Booking_System.UI;
using Airport_Ticket_Booking_System.Managers;
using Airport_Ticket_Booking_System.Interfaces;
using Airport_Ticket_Booking_System.Models;

namespace Airport_Ticket_Booking_System_Test
{
    public class MenuTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IFlightService> _flightServiceMock;
        private readonly Mock<IBookingService> _bookingServiceMock;
        private readonly Menu _menu;

        public MenuTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _flightServiceMock = _fixture.Freeze<Mock<IFlightService>>();
            _bookingServiceMock = _fixture.Freeze<Mock<IBookingService>>();

            var manager = new Manager(_flightServiceMock.Object, _bookingServiceMock.Object);
            _menu = new Menu(manager);
        }

        [Fact]
        public void BookFlight_ShouldBookFlight_WhenFlightExists()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();
            _flightServiceMock.Setup(fs => fs.GetAllFlights()).Returns(new List<Flight> { flight });
            _flightServiceMock.Setup(fs => fs.GetFlightByNumber(It.IsAny<string>())).Returns(flight);

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader($"{flight.FlightNumber}\nEconomy\n"))
                {
                    Console.SetIn(sr);
                    _menu.BookFlight();
                }
            }

            // Assert the call is made with a null third argument since original method returns null.
            _bookingServiceMock.Verify(bs => bs.BookFlight(It.IsAny<string>(), flight, null), Times.Once);
        }

        [Fact]
        public void BookFlight_ShouldNotBookFlight_WhenFlightDoesNotExist()
        {
            // Arrange
            _flightServiceMock.Setup(fs => fs.GetAllFlights()).Returns(new List<Flight>());
            _flightServiceMock.Setup(fs => fs.GetFlightByNumber(It.IsAny<string>())).Returns((Flight)null);

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                using (var sr = new StringReader("NonExistentFlight\nEconomy\n"))
                {
                    Console.SetIn(sr);
                    _menu.BookFlight();
                }
            }

            // Assert
            _bookingServiceMock.Verify(bs => bs.BookFlight(It.IsAny<string>(), It.IsAny<Flight>(), It.IsAny<string>()), Times.Never);
        }
    }
}