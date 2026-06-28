using System;
using System.Collections.Generic;
using System.Linq;
using AB300.Web.Controllers;
using AB300.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace AB300.Web.Tests;

public class HomeControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        var controller = new HomeController();

        var result = controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        var controller = new HomeController();

        var result = controller.Privacy();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsViewResultWithRequestId()
    {
        var controller = new HomeController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        var result = controller.Error();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

        Assert.False(string.IsNullOrEmpty(model.RequestId));
    }
}

public class PlaneControllerTests
{
    [Fact]
    public void Index_ReturnsViewResultWithPlaneList()
    {
        var controller = new PlanesController();

        var result = controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Plane>>(viewResult.Model);

        Assert.NotEmpty(model);
    }

    [Fact]
    public void Details_ReturnsNotFound_ForMissingPlane()
    {
        var controller = new PlanesController();

        var result = controller.Details(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Details_ReturnsViewResult_ForExistingPlane()
    {
        var controller = new PlanesController();

        var result = controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Plane>(viewResult.Model);

        Assert.Equal(1, model.Id);
    }

    [Fact]
    public void Create_Post_ReturnsViewResult_WhenModelStateInvalid()
    {
        var controller = new PlanesController();
        controller.ModelState.AddModelError("Model", "Required");

        var plane = new Plane();

        var result = controller.Create(plane);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(plane, viewResult.Model);
    }

    [Fact]
    public void Create_Post_RedirectsToIndex_WhenModelValid()
    {
        var controller = new PlanesController();
        var plane = new Plane { Model = "Test Plane", Capacity = 100 };

        var result = controller.Create(plane);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlanesController.Index), redirect.ActionName);

        var deleteResult = controller.DeleteConfirmed(plane.Id);
        Assert.IsType<RedirectToActionResult>(deleteResult);
    }

    [Fact]
    public void Edit_Post_ReturnsBadRequest_WhenIdMismatch()
    {
        var controller = new PlanesController();
        var plane = new Plane { Id = 999, Model = "Invalid", Capacity = 1 };

        var result = controller.Edit(1, plane);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Edit_Get_ReturnsNotFound_ForMissingPlane()
    {
        var controller = new PlanesController();

        var result = controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_ReturnsViewResult_WhenModelStateInvalid()
    {
        var controller = new PlanesController();
        controller.ModelState.AddModelError("Model", "Required");

        var plane = new Plane { Id = 1, Model = string.Empty, Capacity = 0 };

        var result = controller.Edit(1, plane);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(plane, viewResult.Model);
    }

    [Fact]
    public void Edit_Post_RedirectsToIndex_WhenModelValid()
    {
        var controller = new PlanesController();
        var plane = new Plane { Id = 1, Model = "Updated", Capacity = 200 };

        var result = controller.Edit(1, plane);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlanesController.Index), redirect.ActionName);
    }

    [Fact]
    public void Delete_ReturnsNotFound_ForMissingPlane()
    {
        var controller = new PlanesController();

        var result = controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteConfirmed_ReturnsNotFound_ForMissingPlane()
    {
        var controller = new PlanesController();

        var result = controller.DeleteConfirmed(999);

        Assert.IsType<NotFoundResult>(result);
    }
}

public class FlightControllerTests
{
    [Fact]
    public void Index_ReturnsViewResultWithFlightList()
    {
        var controller = new FlightsController();

        var result = controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Flight>>(viewResult.Model);

        Assert.NotEmpty(model);
    }

    [Fact]
    public void Details_ReturnsNotFound_ForMissingFlight()
    {
        var controller = new FlightsController();

        var result = controller.Details(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Post_RedirectsToIndex_WhenModelValid()
    {
        var controller = new FlightsController();
        var flight = new Flight
        {
            Origin = "Test",
            Destination = "Target",
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(2)
        };

        var result = controller.Create(flight);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(FlightsController.Index), redirect.ActionName);

        var deleteResult = controller.DeleteConfirmed(flight.Id);
        Assert.IsType<RedirectToActionResult>(deleteResult);
    }

    [Fact]
    public void Edit_Post_ReturnsBadRequest_WhenIdMismatch()
    {
        var controller = new FlightsController();
        var flight = new Flight
        {
            Id = 999,
            Origin = "A",
            Destination = "B",
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(1)
        };

        var result = controller.Edit(1, flight);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void UpdateStatus_ReturnsNotFound_ForMissingFlight()
    {
        var controller = new FlightsController();

        var result = controller.UpdateStatus(999, FlightStatus.Cancelled);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void UpdateStatus_ReturnsBadRequest_ForInvalidTransition()
    {
        var controller = new FlightsController();
        var flight = new Flight
        {
            Origin = "Test",
            Destination = "Target",
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(2)
        };

        var createResult = controller.Create(flight);
        Assert.IsType<RedirectToActionResult>(createResult);

        var departResult = controller.UpdateStatus(flight.Id, FlightStatus.Departed);
        Assert.IsType<RedirectToActionResult>(departResult);

        var badResult = controller.UpdateStatus(flight.Id, FlightStatus.Delayed);
        Assert.IsType<BadRequestObjectResult>(badResult);

        var deleteResult = controller.DeleteConfirmed(flight.Id);
        Assert.IsType<RedirectToActionResult>(deleteResult);
    }

    [Fact]
    public void UpdateStatus_ReturnsRedirect_ForValidTransition()
    {
        var controller = new FlightsController();
        var flight = new Flight
        {
            Origin = "Test",
            Destination = "Target",
            DepartureTime = DateTime.Now,
            ArrivalTime = DateTime.Now.AddHours(2)
        };

        var createResult = controller.Create(flight);
        Assert.IsType<RedirectToActionResult>(createResult);

        var result = controller.UpdateStatus(flight.Id, FlightStatus.Departed);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(FlightsController.Details), redirect.ActionName);
        Assert.Equal(flight.Id, redirect.RouteValues["id"]);

        var deleteResult = controller.DeleteConfirmed(flight.Id);
        Assert.IsType<RedirectToActionResult>(deleteResult);
    }

    [Fact]
    public void Edit_Get_ReturnsNotFound_ForMissingFlight()
    {
        var controller = new FlightsController();

        var result = controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_ReturnsViewResult_WhenModelStateInvalid()
    {
        var controller = new FlightsController();
        controller.ModelState.AddModelError("Origin", "Required");

        var flight = new Flight { Id = 1, Origin = string.Empty, Destination = "B", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(1) };

        var result = controller.Edit(1, flight);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(flight, viewResult.Model);
    }

    [Fact]
    public void Edit_Post_RedirectsToIndex_WhenModelValid()
    {
        var controller = new FlightsController();
        var flight = new Flight { Id = 1, Origin = "A", Destination = "B", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(1) };

        var result = controller.Edit(1, flight);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(FlightsController.Index), redirect.ActionName);
    }

    [Fact]
    public void Delete_ReturnsNotFound_ForMissingFlight()
    {
        var controller = new FlightsController();

        var result = controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteConfirmed_ReturnsNotFound_ForMissingFlight()
    {
        var controller = new FlightsController();

        var result = controller.DeleteConfirmed(999);

        Assert.IsType<NotFoundResult>(result);
    }
}

public class ModelTests
{
    [Fact]
    public void Plane_ModelProperties_CanBeAssigned()
    {
        var plane = new Plane { Id = 10, Model = "Test", Capacity = 50 };

        Assert.Equal(10, plane.Id);
        Assert.Equal("Test", plane.Model);
        Assert.Equal(50, plane.Capacity);
    }

    [Fact]
    public void ErrorViewModel_CanStoreRequestId()
    {
        var model = new ErrorViewModel { RequestId = "request-123" };

        Assert.Equal("request-123", model.RequestId);
    }

    [Fact]
    public void Flight_StatusTransition_AllowsBoardingFromScheduled()
    {
        var flight = new Flight { Status = FlightStatus.Scheduled };

        var result = flight.TryUpdateStatus(FlightStatus.Boarding);

        Assert.True(result);
        Assert.Equal(FlightStatus.Boarding, flight.Status);
    }

    [Fact]
    public void Flight_StatusTransition_RejectsDelayedAfterArrived()
    {
        var flight = new Flight { Status = FlightStatus.Arrived };

        var result = flight.TryUpdateStatus(FlightStatus.Delayed);

        Assert.False(result);
        Assert.Equal(FlightStatus.Arrived, flight.Status);
    }
}
