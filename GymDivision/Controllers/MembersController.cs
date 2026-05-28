using GymDivision.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymDivision.Controllers;

[Route("Home/Members")]
public class MembersController : Controller
{
    private readonly MembersContext membersContext;

    public MembersController(MembersContext membersContext)
    {
        this.membersContext = membersContext;
    }
    
    [HttpGet("GetAllMembers")]
    public IActionResult GetAllMembers()
    {
        using var db = membersContext;
        var members = db.Members.ToList();
        return Ok(members);
    }
    
    
    [HttpGet("GetMember")]
    public IActionResult GetMember(string name)
    {
        using var db = membersContext;
        var member = db.Members.FirstOrDefault(x => x.Name == name);
        if (member == null) return NotFound();
        return Ok(member);
    }
    
    
    [HttpPost("AddMember")]
    public IActionResult AddMember(MemberModel member)
    {
        using var db = membersContext;
        db.Members.Add(member);
        db.SaveChanges();
        return Redirect($"/Home/Members");
    }

    
    [HttpPost("DeleteMember")]
    public IActionResult DeleteMember(int id)
    {
        using var db = membersContext;
        var member = db.Members.FirstOrDefault(x => x.Id == id);
        if (member == null) return NotFound();
        
        db.Members.Remove(member);
        db.SaveChanges();
        return Ok();
    }

    [HttpPost("EditMember")]
    public IActionResult EditMember(MemberModel member)
    {
        using var db = membersContext;
        var memberToUpdate = db.Members.FirstOrDefault(x => x.Id == member.Id);
        if (memberToUpdate == null) return NotFound();
        
        memberToUpdate.Name = member.Name;
        memberToUpdate.Level = member.Level;
        memberToUpdate.Weight = member.Weight;
        memberToUpdate.Height = member.Height;
        memberToUpdate.Age = member.Age;
        db.SaveChanges();
        
        return Redirect($"/Home/Members");
    }
}