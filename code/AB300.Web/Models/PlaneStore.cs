using System.Collections.Generic;

namespace AB300.Web.Models;

internal static class PlaneStore
{
    internal static readonly List<Plane> Planes = new()
    {
        new Plane { Id = 1, Model = "Boeing 737", Capacity = 189 },
        new Plane { Id = 2, Model = "Airbus A320", Capacity = 180 }
    };

    internal static void Reset()
    {
        Planes.Clear();
        Planes.AddRange(new[]
        {
            new Plane { Id = 1, Model = "Boeing 737", Capacity = 189 },
            new Plane { Id = 2, Model = "Airbus A320", Capacity = 180 }
        });
    }
}
