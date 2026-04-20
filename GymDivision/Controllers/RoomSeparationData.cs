using GymDivision.Models;

namespace GymDivision.Controllers;

public struct RoomSeparationData(int roomSizeIdeal, int roomSizeMin, int roomSizeMax, List<MemberModel> members)
{
    public int MembersToAdd { get; set; } = 0;
    public int RoomSizeMin { get; private set; } = roomSizeMin;
    public int RoomSizeMax { get; private set; } = roomSizeMax;
    public int RoomSizeIdeal { get; private set; } = roomSizeIdeal;
    public List<MemberModel> Members { get; set; } = members;
}