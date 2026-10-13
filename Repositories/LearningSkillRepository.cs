using Microsoft.EntityFrameworkCore;
using SkillSwapBackend.Data;
using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public class LearningSkillRepository : ILearningSkillRepository
{
    private readonly AppDbContext _context;

    public LearningSkillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LearningSkill> CreateLearningSkill(LearningSkill learningSkill)
    {
        _context.LearningSkills.Add(learningSkill);
        await _context.SaveChangesAsync();

        return learningSkill;
    }

    public async Task<List<LearningSkill>> GetLearningSkillsByUserId(int userId)
    {
        return await _context.LearningSkills
            .Where(learningSkill => learningSkill.UserId == userId)
            .ToListAsync();
    }

    public async Task<LearningSkill?> GetLearningSkillById(int id)
    {
        return await _context.LearningSkills.FindAsync(id);
    }

    public async Task<LearningSkill?> UpdateLearningSkill(
        int id,
        LearningSkill learningSkill)
    {
        var existingLearningSkill = await _context.LearningSkills.FindAsync(id);

        if (existingLearningSkill == null)
        {
            return null;
        }

        existingLearningSkill.SkillName = learningSkill.SkillName;

        await _context.SaveChangesAsync();

        return existingLearningSkill;
    }

    public async Task<bool> DeleteLearningSkill(int id)
    {
        var learningSkill = await _context.LearningSkills.FindAsync(id);

        if (learningSkill == null)
        {
            return false;
        }

        _context.LearningSkills.Remove(learningSkill);
        await _context.SaveChangesAsync();

        return true;
    }
}
