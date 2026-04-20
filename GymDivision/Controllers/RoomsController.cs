using GymDivision.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymDivision.Controllers;

[Route("Home/Rooms")]
public class RoomsController : Controller
{
    /// <summary>
    /// Get all available rooms in the gym
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAllRooms")]
    public IActionResult GetAllRooms()
    {
        using var db = new RoomContext();
        var rooms = db.Rooms.ToList();
        return Ok(rooms);
    }

    
    /// <summary>
    /// Get a specific room by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetRoom")]
    public IActionResult GetRoom(int id)
    {
        using var db = new RoomContext();
        var room = db.Rooms.FirstOrDefault(x => x.Id == id);
        if (room == null) return NotFound();
        return Ok(room);
    }
    
    
    /// <summary>
    /// Add a new room to the gym
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    [HttpPost("AddRoom")]
    public IActionResult AddRoom(RoomModel room)
    {
        if (!AreValidRoomMembers()) return BadRequest();

        using var db = new RoomContext();
        db.Rooms.Add(room);
        db.SaveChanges();
        return Redirect($"/Home/ViewRooms");
        
        bool AreValidRoomMembers()
        {
            return room.MinMembers <= room.MaxMembers && 
                   room.MinMembers <= room.IdealMembers && 
                   room.MaxMembers >= room.IdealMembers;
        }
    }

    
    /// <summary>
    /// Remove a room from the gym
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("RemoveRoom")]
    public IActionResult RemoveRoom(int id)
    {
        using var db = new RoomContext();
        var room = db.Rooms.FirstOrDefault(x => x.Id == id);
        if (room == null) return NotFound();
        
        db.Rooms.Remove(room);
        db.SaveChanges();
        return Ok();
    }


    /// <summary>
    /// Edit a room
    /// </summary>
    /// <param name="roomModel"></param>
    /// <returns></returns>
    [HttpPost("EditRoom")]
    public IActionResult EditRoom(RoomModel roomModel)
    {
        using var db = new RoomContext();
        var room = db.Rooms.FirstOrDefault(x => x.Id == roomModel.Id);
        if (room == null) return NotFound();
        
        room.MinMembers = roomModel.MinMembers;
        room.MaxMembers = roomModel.MaxMembers;
        room.IdealMembers = roomModel.IdealMembers;
        db.SaveChanges();
        
        return Redirect($"/Home/ViewRooms");
    }
}