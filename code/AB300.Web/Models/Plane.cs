using System.ComponentModel.DataAnnotations;

namespace AB300.Web.Models;

public class Plane
{
    public int Id { get; set; }

    [Required]
    public string Model { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero.")]
    public int Capacity { get; set; }
}
