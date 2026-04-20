using System.ComponentModel.DataAnnotations;

namespace GymDivision.Models;

public class RoomModel
{
    public int Id { get; set; }
    [Range(1, 100, ErrorMessage = "Members must be between 1 and 100 ")]
    public int IdealMembers { get; set; }
    [Range(1, 100, ErrorMessage = "Members must be between 1 and 100")]
    public int MaxMembers { get; set; }
    [Range(1, 100, ErrorMessage = "Members must be between 1 and 100")]
    public int MinMembers { get; set; }
}