using Microsoft.AspNetCore.Mvc;
using SkillSwapBackend.Models;
using SkillSwapBackend.Repositories;
using SkillSwapBackend.ViewModels;

namespace SkillSwapBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController : ControllerBase
{
    private readonly ISkillRepository _skillRepository;

    public SkillController(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> CreateSkill(SkillViewModel model)
    {
        var skill = new Skill
        {
            UserId = model.UserId,
            SkillName = model.SkillName,
            SkillType = model.SkillType,
            SkillLevel = model.SkillLevel
        };

        var createdSkill =
            await _skillRepository.CreateSkill(skill);

        return Ok(createdSkill);
    }

    // READ USER SKILLS
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetSkillsByUserId(int userId)
    {
        var skills =
            await _skillRepository.GetSkillsByUserId(userId);

        return Ok(skills);
    }

    [HttpGet("~/api/skills/users")]
    public async Task<IActionResult> GetUsersBySkillName(
        [FromQuery] string skillName)
    {
        if (string.IsNullOrWhiteSpace(skillName))
        {
            return BadRequest("skillName is required");
        }

        var users = await _skillRepository.GetUsersBySkillName(skillName.Trim());

        return Ok(users.Select(user => new
        {
            user.Id,
            user.FullName,
            user.Username,
            user.Email,
            user.MobileNumber
        }));
    }

    // READ ONE
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSkillById(int id)
    {
        var skill =
            await _skillRepository.GetSkillById(id);

        if (skill == null)
        {
            return NotFound("Skill not found");
        }

        return Ok(skill);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSkill(
        int id,
        SkillViewModel model)
    {
        var skill = new Skill
        {
            UserId = model.UserId,
            SkillName = model.SkillName,
            SkillType = model.SkillType,
            SkillLevel = model.SkillLevel
        };

        var updatedSkill =
            await _skillRepository.UpdateSkill(id, skill);

        if (updatedSkill == null)
        {
            return NotFound("Skill not found");
        }

        return Ok(updatedSkill);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var deleted =
            await _skillRepository.DeleteSkill(id);

        if (!deleted)
        {
            return NotFound("Skill not found");
        }

        return Ok("Skill deleted successfully");
    }
}