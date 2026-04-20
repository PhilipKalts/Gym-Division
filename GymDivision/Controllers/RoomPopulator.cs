using GymDivision.Models;

namespace GymDivision.Controllers;

public struct RoomPopulator(List<MemberModel> memberDatas, RoomSeparationData[] roomSeparationDatas)
{
    List<MemberModel> memberDatas = memberDatas;
    RoomSeparationData[] roomSeparationDatas = roomSeparationDatas;
    
    
    /// <summary>
    /// Populate the rooms based on how many members should be in each room
    /// The members are entered in the order they are in the list.
    /// Since the list is sorted by level, the members are entered in the order they are in the list.
    /// </summary>
    /// <returns></returns>
    public RoomSeparationData[] GetResult()
    {
        int memberIndex = 0;
        for (int i = 0; i < roomSeparationDatas.Length; i++)
        {
            for (int j = 0; j < roomSeparationDatas[i].MembersToAdd; j++)
            {
                roomSeparationDatas[i].Members.Add(memberDatas[memberIndex]);
                memberIndex++;
            }
        }
        return roomSeparationDatas;
    }
}