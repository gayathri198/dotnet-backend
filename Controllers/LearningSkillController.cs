using Microsoft.AspNetCore.Mvc;
using SkillSwapBackend.Models;
using SkillSwapBackend.Repositories;
using SkillSwapBackend.ViewModels;

namespace SkillSwapBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearningSkillController : ControllerBase
{
    private readonly ILearningSkillRepository _learningSkillRepository;
    private readonly IUserRepository _userRepository;

    public LearningSkillController(
        ILearningSkillRepository learningSkillRepository,
        IUserRepository userRepository)
    {
        _learningSkillRepository = learningSkillRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLearningSkill(LearningSkillViewModel model)
    {
        if (await _userRepository.GetUserById(model.UserId) == null)
        {
            return BadRequest("User not found");
        }

        var learningSkill = new LearningSkill
        {
            UserId = model.UserId,
            SkillName = model.SkillName
        };

        var createdLearningSkill =
            await _learningSkillRepository.CreateLearningSkill(learningSkill);

        return CreatedAtAction(
            nameof(GetLearningSkillById),
            new { id = createdLearningSkill.LearningSkillId },
            createdLearningSkill);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetLearningSkillsByUserId(int userId)
    {
        var learningSkills =
            await _learningSkillRepository.GetLearningSkillsByUserId(userId);

        return Ok(learningSkills);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLearningSkillById(int id)
    {
        var learningSkill =
            await _learningSkillRepository.GetLearningSkillById(id);

        if (learningSkill == null)
        {
            return NotFound("Learning skill not found");
        }

        return Ok(learningSkill);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLearningSkill(
        int id,
        LearningSkillViewModel model)
    {
        var learningSkill = new LearningSkill
        {
            UserId = model.UserId,
            SkillName = model.SkillName
        };

        var updatedLearningSkill =
            await _learningSkillRepository.UpdateLearningSkill(id, learningSkill);

        if (updatedLearningSkill == null)
        {
            return NotFound("Learning skill not found");
        }

        return Ok(updatedLearningSkill);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLearningSkill(int id)
    {
        var deleted = await _learningSkillRepository.DeleteLearningSkill(id);

        if (!deleted)
        {
            return NotFound("Learning skill not found");
        }

        return Ok("Learning skill deleted successfully");
    }
}
