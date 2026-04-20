using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymDivision.Models;

namespace GymDivision.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Members()
    {
        var members = new Context().Members.ToList();
        return View(members);
    }
    
    public IActionResult ViewRooms()
    {
        var rooms = new RoomContext().Rooms.ToList();
        return View(rooms);
    }

    public IActionResult Generate()
    {
        var members = new Context().Members.ToList();
        var rooms = new RoomContext().Rooms.ToList();
        
        GenerateViewModel model = new(members, rooms);
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}