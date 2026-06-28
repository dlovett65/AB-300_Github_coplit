    using System.ComponentModel.DataAnnotations;

namespace AB300.Web.Models;

public enum FlightStatus
{
    Scheduled,
    Boarding,
    Delayed,
    Departed,
    InFlight,
    Landed,
    Arrived,
    Cancelled
}

public class Flight
{
    public int Id { get; set; }

    [Required]
    public string Origin { get; set; } = string.Empty;

    [Required]
    public string Destination { get; set; } = string.Empty;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    public bool TryUpdateStatus(FlightStatus newStatus)
    {
        if (newStatus == Status)
        {
            return true;
        }

        return newStatus switch
        {
            FlightStatus.Scheduled => Status is not FlightStatus.Delayed and not FlightStatus.Cancelled,
            FlightStatus.Boarding => Status is FlightStatus.Scheduled or FlightStatus.Delayed,
            FlightStatus.Delayed => Status == FlightStatus.Scheduled,
            FlightStatus.Departed => Status is FlightStatus.Scheduled or FlightStatus.Delayed,
            FlightStatus.InFlight => Status == FlightStatus.Departed,
            FlightStatus.Landed => Status == FlightStatus.Departed,
            FlightStatus.Arrived => Status == FlightStatus.Departed,
            FlightStatus.Cancelled => Status != FlightStatus.Cancelled,
            _ => false,
        } && SetStatus(newStatus);
    }

    private bool SetStatus(FlightStatus newStatus)
    {
        Status = newStatus;
        return true;
    }
}