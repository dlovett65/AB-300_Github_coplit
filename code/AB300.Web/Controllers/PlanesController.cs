using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AB300.Web.Models;

namespace AB300.Web.Controllers;

public class PlanesController : Controller
{
    public IActionResult Index()
    {
        return View(PlaneStore.Planes);
    }

    public IActionResult Details(int id)
    {
        var plane = PlaneStore.Planes.FirstOrDefault(p => p.Id == id);
        if (plane == null)
        {
            return NotFound();
        }

        return View(plane);
    }

    public IActionResult Create()
    {
        return View(new Plane());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Plane plane)
    {
        if (!ModelState.IsValid)
        {
            return View(plane);
        }

        plane.Id = PlaneStore.Planes.Any() ? PlaneStore.Planes.Max(p => p.Id) + 1 : 1;
        PlaneStore.Planes.Add(plane);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var plane = PlaneStore.Planes.FirstOrDefault(p => p.Id == id);
        if (plane == null)
        {
            return NotFound();
        }

        return View(plane);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Plane plane)
    {
        if (id != plane.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(plane);
        }

        var existingPlane = PlaneStore.Planes.FirstOrDefault(p => p.Id == id);
        if (existingPlane == null)
        {
            return NotFound();
        }

        existingPlane.Model = plane.Model;
        existingPlane.Capacity = plane.Capacity;

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var plane = PlaneStore.Planes.FirstOrDefault(p => p.Id == id);
        if (plane == null)
        {
            return NotFound();
        }

        return View(plane);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var plane = PlaneStore.Planes.FirstOrDefault(p => p.Id == id);
        if (plane == null)
        {
            return NotFound();
        }

        PlaneStore.Planes.Remove(plane);
        return RedirectToAction(nameof(Index));
    }
}
