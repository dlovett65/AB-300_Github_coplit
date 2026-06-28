using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAviationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanesController : ControllerBase
    {
        // In-memory data store for demo purposes
        private static readonly List<Plane> _planes = new List<Plane>
        {
            new Plane { Id = 1, Model = "Boeing 737", Capacity = 189 },
            new Plane { Id = 2, Model = "Airbus A320", Capacity = 180 }
        };

        // GET: api/planes
        [HttpGet]
        public ActionResult<IEnumerable<Plane>> GetPlanes()
        {
            return Ok(_planes);
        }

        // GET: api/planes/{id}
        [HttpGet("{id:int}")]
        public ActionResult<Plane> GetPlane(int id)
        {
            var plane = _planes.FirstOrDefault(p => p.Id == id);
            if (plane == null)
                return NotFound(new { message = $"Plane with ID {id} not found." });

            return Ok(plane);
        }

        // POST: api/planes
        [HttpPost]
        public ActionResult<Plane> CreatePlane([FromBody] Plane newPlane)
        {
            if (newPlane == null || string.IsNullOrWhiteSpace(newPlane.Model) || newPlane.Capacity <= 0)
                return BadRequest(new { message = "Invalid plane data." });

            newPlane.Id = _planes.Any() ? _planes.Max(p => p.Id) + 1 : 1;
            _planes.Add(newPlane);

            return CreatedAtAction(nameof(GetPlane), new { id = newPlane.Id }, newPlane);
        }

        // PUT: api/planes/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdatePlane(int id, [FromBody] Plane updatedPlane)
        {
            if (updatedPlane == null || string.IsNullOrWhiteSpace(updatedPlane.Model) || updatedPlane.Capacity <= 0)
                return BadRequest(new { message = "Invalid plane data." });

            var existingPlane = _planes.FirstOrDefault(p => p.Id == id);
            if (existingPlane == null)
                return NotFound(new { message = $"Plane with ID {id} not found." });

            existingPlane.Model = updatedPlane.Model;
            existingPlane.Capacity = updatedPlane.Capacity;

            return NoContent();
        }

        // DELETE: api/planes/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeletePlane(int id)
        {
            var plane = _planes.FirstOrDefault(p => p.Id == id);
            if (plane == null)
                return NotFound(new { message = $"Plane with ID {id} not found." });

            _planes.Remove(plane);
            return NoContent();
        }
    }

    // Plane model
    public class Plane
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
    }
}
