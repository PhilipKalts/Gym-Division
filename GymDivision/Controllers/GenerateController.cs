using GymDivision.Domain;
using GymDivision.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymDivision.Controllers;

public enum RequirementType { Level, Age, Injury, Pair }

[Route("Home/Generate")]
public class GenerateController : Controller
{
    private readonly ILogger<GenerateController> logger;
    public GenerateController(ILogger<GenerateController> logger) => this.logger = logger;
    List<MemberModel> memberDatas;
    RoomSeparationData[] roomSeparationDatas;


    /// <summary>
    /// Generate the rooms and members
    /// </summary>
    /// <param name="memberIds"></param>
    /// <param name="requirements"></param>
    /// <returns></returns>
    [HttpGet("Generate")]
    public IActionResult Generate(List<int> memberIds, List<RequirementType> requirements)
    {
        //TODO: remove after testing
        Console.WriteLine(requirements.Count);
        for (int i = 0; i < requirements.Count; i++)
        {
            Console.WriteLine(requirements[i]);
        }
        bool shouldUseNew = true;
        {
            memberIds.Clear();
            memberIds = new List<int>();
            for (int i = 0; i <= 18; i++) memberIds.Add(i);
        }
        
        if (memberIds.Count == 0) return BadRequest("Member IDs cannot be null or empty");
        
        PopulateMemberData(memberIds);
        PopulateTheRoomSeparationData();
        if (!IsThereSpaceForAllMembers()) 
            return BadRequest("You entered too many members");
        
        MemberDistributor memberDistributor = new(memberDatas.Count, roomSeparationDatas);
        memberDistributor.SetDistribution();

        if (shouldUseNew)
        {
            RoomPopulator roomPopulator = new(memberDatas, roomSeparationDatas);
            roomPopulator.SetRoomSeparations();
        }
        else
        {
            SimpleRoomPopulator simpleRoomPopulator = new(memberDatas, roomSeparationDatas);
            simpleRoomPopulator.SetResultBasedOnLevel();
        }
        
        
        return Json(roomSeparationDatas);
    }
    
    
    /// <summary>
    /// Populate the memberDatas with the member data from the database
    /// </summary>
    /// <param name="memberIds"></param>
    void PopulateMemberData(List<int> memberIds)
    {
        using var db = new Context();
        memberDatas = db.Members
            .Where(x => memberIds.Contains(x.Id))
            .OrderBy(x => x.Level)
            .ToList();
    }
    

    /// <summary>
    /// Populate the roomSeparationDatas with the room data from the database
    /// </summary>
    void PopulateTheRoomSeparationData()
    {
        using var dbRooms = new RoomContext();
        var allRooms = dbRooms.Rooms.ToList();
        roomSeparationDatas = new RoomSeparationData[allRooms.Count];

        for (int i = 0; i < allRooms.Count; i++)
        {
            roomSeparationDatas[i] = new RoomSeparationData(
                allRooms[i].IdealMembers, 
                allRooms[i].MinMembers, 
                allRooms[i].MaxMembers, 
                new List<MemberModel>());
        }
    }
    
    
    /// <summary>
    /// Check if there is enough space for all members
    /// </summary>
    /// <returns></returns>
    bool IsThereSpaceForAllMembers()
    {
        int maxMembersCanEnter = roomSeparationDatas.Sum(x => x.RoomSizeMax);
        bool isSpaceEnough = memberDatas.Count <= maxMembersCanEnter;
        return isSpaceEnough;
    }
}