using GymDivision.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymDivision.Controllers;

[Route("Home/Rooms")]
public class RoomsController : Controller
{
    readonly RoomsContext roomsContext;

    public RoomsController(RoomsContext roomsContext)
    {
        this.roomsContext = roomsContext;
    }
    
    /// <summary>
    /// Get all available rooms in the gym
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAllRooms")]
    public IActionResult GetAllRooms()
    {
        var rooms = roomsContext.Rooms.ToList();
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
        var room = roomsContext.Rooms.FirstOrDefault(x => x.Id == id);
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

        roomsContext.Rooms.Add(room);
        roomsContext.SaveChanges();
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
        var room = roomsContext.Rooms.FirstOrDefault(x => x.Id == id);
        if (room == null) return NotFound();
        
        roomsContext.Rooms.Remove(room);
        roomsContext.SaveChanges();
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
        var room = roomsContext.Rooms.FirstOrDefault(x => x.Id == roomModel.Id);
        if (room == null) return NotFound();
        
        room.MinMembers = roomModel.MinMembers;
        room.MaxMembers = roomModel.MaxMembers;
        room.IdealMembers = roomModel.IdealMembers;
        roomsContext.SaveChanges();
        
        return Redirect($"/Home/ViewRooms");
    }
}