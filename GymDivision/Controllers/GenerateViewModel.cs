using GymDivision.Models;

namespace GymDivision.Controllers;

public class GenerateViewModel(List<MemberModel> members, List<RoomModel> rooms)
{
    public List<MemberModel> Members { get; set; } = members;
    public List<RoomModel> Rooms { get; set; } = rooms;
}