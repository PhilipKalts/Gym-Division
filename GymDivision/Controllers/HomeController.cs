using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymDivision.Models;

namespace GymDivision.Controllers;

public class HomeController : Controller
{
    readonly RoomsContext roomsContext;
    readonly MembersContext membersContext;

    public HomeController(MembersContext membersContext, RoomsContext roomsContext)
    {
        this.roomsContext = roomsContext;
        this.membersContext = membersContext;
    }
    
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
        var members = membersContext.Members.ToList();
        return View(members);
    }
    
    public IActionResult ViewRooms()
    {
        var rooms = roomsContext.Rooms.ToList();
        return View(rooms);
    }

    public IActionResult Generate()
    {
        var rooms = roomsContext.Rooms.ToList();
        var members = membersContext.Members.ToList();
        
        GenerateViewModel model = new(members, rooms);
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}