using System.Diagnostics;
using GymDivision.Models;

namespace GymDivision.Controllers;

public class SimpleRoomPopulator(List<MemberModel> allMembers, RoomSeparationData[] roomSeparationDatas)
{
    List<MemberModel> allMembers = allMembers;
    RoomSeparationData[] roomSeparationDatas = roomSeparationDatas;
    
    /// <summary>
    /// Populate the rooms based on how many members should be in each room
    /// The members are entered in the order they are in the list.
    /// Since the list is sorted by level, the members are entered in the order they are in the list.
    /// </summary>
    /// <returns></returns>
    public void SetResultBasedOnLevel()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        int memberIndex = 0;
        allMembers = allMembers.OrderBy(x => x.Level).ToList();
        
        for (int i = 0; i < roomSeparationDatas.Length; i++)
        {
            for (int j = 0; j < roomSeparationDatas[i].MembersToAdd; j++)
            {
                roomSeparationDatas[i].Members.Add(allMembers[memberIndex]);
                memberIndex++;
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
    }
}