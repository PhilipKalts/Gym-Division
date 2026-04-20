using System.ComponentModel;

namespace GymDivision.Models;

public class MemberModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Level { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public int Age { get; set; }
    public string Notes { get; set; } = "";
}