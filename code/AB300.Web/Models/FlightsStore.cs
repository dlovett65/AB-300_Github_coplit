using System.Collections.Generic;

namespace AB300.Web.Models;

public static class FlightStore
{
    public static readonly List<Flight> Flights = new()
    {
        new Flight { Id = 1, Origin = "New York", Destination = "Los Angeles", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(5) },
        new Flight { Id = 2, Origin = "Chicago", Destination = "Miami", DepartureTime = DateTime.Now.AddHours(2), ArrivalTime = DateTime.Now.AddHours(7) }
    };

    public static void Reset()
    {
        Flights.Clear();
        Flights.AddRange(new[]
        {
                new Flight { Id = 1, Origin = "New York", Destination = "Los Angeles", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(5) },
                new Flight { Id = 2, Origin = "Chicago", Destination = "Miami", DepartureTime = DateTime.Now.AddHours(2), ArrivalTime = DateTime.Now.AddHours(7) }
        });
    }
}