using System.ComponentModel;

namespace GymDivision.Models;

public enum Gender { Male, Female }
public enum Injury { None, Neck, Back, Shoulders, Arms, Legs }

public class MemberModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Gender Gender { get; set; }
    public int Level { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public int Age { get; set; }
    public Injury Injury { get; set; }
    public string PairMember { get; set; } = "";
    public string Notes { get; set; } = "";
}