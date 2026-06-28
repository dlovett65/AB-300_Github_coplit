using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AB300.Web.Models;

namespace AB300.Web.Controllers;

public class FlightsController : Controller
{
    public IActionResult Index()
    {
        return View(FlightStore.Flights);
    }

    public IActionResult Details(int id)
    {
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == id);
        if (flight == null)
        {
            return NotFound();
        }

        return View(flight);
    }

    public IActionResult Create()
    {
        return View(new Flight());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Flight flight)
    {
        if (!ModelState.IsValid)
        {
            return View(flight);
        }

        flight.Id = FlightStore.Flights.Any() ? FlightStore.Flights.Max(p => p.Id) + 1 : 1;
        FlightStore.Flights.Add(flight);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == id);
        if (flight == null)
        {
            return NotFound();
        }

        return View(flight);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Flight flight)
    {
        if (id != flight.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(flight);
        }

        var existingFlight = FlightStore.Flights.FirstOrDefault(f => f.Id == id);
        if (existingFlight == null)
        {
            return NotFound();
        }

        existingFlight.Origin = flight.Origin;
        existingFlight.Destination = flight.Destination;
        existingFlight.DepartureTime = flight.DepartureTime;
        existingFlight.ArrivalTime = flight.ArrivalTime;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == id);
        if (flight == null)
        {
            return NotFound();
        }

        return View(flight);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == id);
        if (flight == null)
        {
            return NotFound();
        }

        FlightStore.Flights.Remove(flight);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost("{id}/status")]
    public IActionResult UpdateStatus(int id, FlightStatus newStatus)
    {
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == id);
        if (flight == null)
        {
            return NotFound();
        }

        if (!flight.TryUpdateStatus(newStatus))
        {
            return BadRequest("Invalid status transition.");
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("{id}/calculateAerodynamics")]
    public IActionResult CalculateAerodynamics(int id)
    {
        var flight = FlightStore.Flights.FirstOrDefault(f => f.Id == id);
        if (flight == null)
        {
            return NotFound();
        }

        // Placeholder for aerodynamics calculation logic
        var aerodynamicsData = new
        {
            Lift = 1000,
            Drag = 200,
            Thrust = 1500
        };

        List<int> results = CalculatePrimes.GetPrimes(2, 25000);
        return Ok(aerodynamicsData);
    }

    public static class CalculatePrimes
    {
        public static List<int> GetPrimes(int start, int end)
        {
            if (start < 2)
            {
                start = 2;
            }

            int max = Math.Max(start, end);
            var isPrime = new bool[max + 1];

            for (int i = 2; i <= max; i++)
            {
                isPrime[i] = true;
            }

            for (int i = 2; i * i <= max; i++)
            {
                if (!isPrime[i])
                {
                    continue;
                }

                for (int j = i * i; j <= max; j += i)
                {
                    isPrime[j] = false;
                }
            }

            var primes = new List<int>();
            for (int number = start; number <= end; number++)
            {
                if (isPrime[number])
                {
                    primes.Add(number);
                }
            }

            return primes;
        }
    }
}

