using System;
using System.Collections.Generic;
using System.Linq;
using AB300.Web.Controllers;
using AB300.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AB300.Web.Tests;

public class FlightsControllerTests
{
    private void ResetFlights()
    {
        FlightStore.Reset();
    }

    [Fact]
    public void Index_ReturnsViewResultWithAllFlights()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<Flight>>(viewResult.Model);
        Assert.NotEmpty(model);
    }

    [Fact]
    public void Details_WithValidId_ReturnsViewResultWithFlight()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int validId = 1;

        // Act
        var result = controller.Details(validId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Flight>(viewResult.Model);
        Assert.Equal(validId, model.Id);
    }

    [Fact]
    public void Details_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int invalidId = 9999;

        // Act
        var result = controller.Details(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Get_ReturnsViewResultWithNewFlight()
    {
        // Arrange
        var controller = new FlightsController();

        // Act
        var result = controller.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Flight>(viewResult.Model);
        Assert.Equal(0, model.Id);
    }

    [Fact]
    public void Create_Post_WithValidFlight_RedirectsToIndex()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        var newFlight = new Flight
        {
            Origin = "Boston",
            Destination = "Denver",
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(4)
        };

        // Act
        var result = controller.Create(newFlight);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(FlightsController.Index), redirectResult.ActionName);
        Assert.Contains(newFlight, FlightStore.Flights);
    }

    [Fact]
    public void Create_Post_WithInvalidFlight_ReturnsViewWithModel()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        var invalidFlight = new Flight { Origin = "", Destination = "" }; // Missing required fields
        controller.ModelState.AddModelError("Origin", "Required");

        // Act
        var result = controller.Create(invalidFlight);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Flight>(viewResult.Model);
        Assert.Equal(invalidFlight, model);
    }

    [Fact]
    public void Edit_Get_WithValidId_ReturnsViewResultWithFlight()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int validId = 1;

        // Act
        var result = controller.Edit(validId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Flight>(viewResult.Model);
        Assert.Equal(validId, model.Id);
    }

    [Fact]
    public void Edit_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int invalidId = 9999;

        // Act
        var result = controller.Edit(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_WithIdMismatch_ReturnsBadRequest()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        var flight = new Flight { Id = 2, Origin = "Seattle", Destination = "Portland" };

        // Act
        var result = controller.Edit(1, flight); // ID mismatch

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Edit_Post_WithValidFlight_RedirectsToIndex()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int flightId = 1;
        var updatedFlight = new Flight
        {
            Id = flightId,
            Origin = "San Francisco",
            Destination = "Seattle",
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(2)
        };

        // Act
        var result = controller.Edit(flightId, updatedFlight);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(FlightsController.Index), redirectResult.ActionName);
        var updatedFlightInStore = FlightStore.Flights.FirstOrDefault(f => f.Id == flightId);
        Assert.NotNull(updatedFlightInStore);
        Assert.Equal("San Francisco", updatedFlightInStore.Origin);
        Assert.Equal("Seattle", updatedFlightInStore.Destination);
    }

    [Fact]
    public void Edit_Post_WithInvalidFlight_ReturnsViewWithModel()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int flightId = 1;
        var invalidFlight = new Flight { Id = flightId, Origin = "", Destination = "" };
        controller.ModelState.AddModelError("Origin", "Required");

        // Act
        var result = controller.Edit(flightId, invalidFlight);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(controller.ModelState.IsValid);
    }

    [Fact]
    public void Edit_Post_WithNonexistentFlight_ReturnsNotFound()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int nonexistentId = 9999;
        var flight = new Flight { Id = nonexistentId, Origin = "Test", Destination = "Test" };

        // Act
        var result = controller.Edit(nonexistentId, flight);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_Get_WithValidId_ReturnsViewResultWithFlight()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int validId = 1;

        // Act
        var result = controller.Delete(validId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Flight>(viewResult.Model);
        Assert.Equal(validId, model.Id);
    }

    [Fact]
    public void Delete_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int invalidId = 9999;

        // Act
        var result = controller.Delete(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_Post_WithValidId_RedirectsToIndexAndRemovesFlight()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int flightId = 1;
        int initialCount = FlightStore.Flights.Count;

        // Act
        var result = controller.DeleteConfirmed(flightId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(FlightsController.Index), redirectResult.ActionName);
        Assert.Equal(initialCount - 1, FlightStore.Flights.Count);
        Assert.DoesNotContain(FlightStore.Flights, f => f.Id == flightId);
    }

    [Fact]
    public void Delete_Post_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int invalidId = 9999;

        // Act
        var result = controller.DeleteConfirmed(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void UpdateStatus_WithValidTransition_RedirectsToDetails()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int flightId = 1;
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == flightId);
        Assert.NotNull(flight);

        // Act
        var result = controller.UpdateStatus(flightId, FlightStatus.Boarding);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(FlightsController.Details), redirectResult.ActionName);
        Assert.Equal(FlightStatus.Boarding, flight.Status);
    }

    [Fact]
    public void UpdateStatus_WithInvalidTransition_ReturnsBadRequest()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int flightId = 1;
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == flightId);
        Assert.NotNull(flight);
        flight.Status = FlightStatus.Cancelled;

        // Act - Cancelled flights cannot change status
        var result = controller.UpdateStatus(flightId, FlightStatus.Boarding);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UpdateStatus_WithNonexistentFlight_ReturnsNotFound()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int invalidId = 9999;

        // Act
        var result = controller.UpdateStatus(invalidId, FlightStatus.Boarding);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CalculateAerodynamics_WithValidId_ReturnsOkWithAerodynamicsData()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int validId = 1;

        // Act
        var result = controller.CalculateAerodynamics(validId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        // Verify the result contains aerodynamics data
        var jsonElement = System.Text.Json.JsonSerializer.SerializeToElement(okResult.Value);
        Assert.True(jsonElement.TryGetProperty("Lift", out var lift) && lift.GetInt32() == 1000);
        Assert.True(jsonElement.TryGetProperty("Drag", out var drag) && drag.GetInt32() == 200);
        Assert.True(jsonElement.TryGetProperty("Thrust", out var thrust) && thrust.GetInt32() == 1500);
    }

    [Fact]
    public void CalculateAerodynamics_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        ResetFlights();
        var controller = new FlightsController();
        int invalidId = 9999;

        // Act
        var result = controller.CalculateAerodynamics(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CalculatePrimes_GetPrimes_ReturnsCorrectPrimes()
    {
        // Arrange
        var expected = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19, 23 };

        // Act
        var result = FlightsController.CalculatePrimes.GetPrimes(2, 25);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculatePrimes_GetPrimes_WithStartLessThanTwo_DefaultsToTwo()
    {
        // Arrange
        var expected = new List<int> { 2, 3, 5, 7 };

        // Act
        var result = FlightsController.CalculatePrimes.GetPrimes(0, 10);

        // Assert
        Assert.Equal(expected, result);
    }
}
