using GymDivision.Models;

namespace GymDivision;

public class Member(MemberModel model)
{
    public MemberModel Model { get; private set; } = model;
    public int ScoreInRoom { get; set; }
}