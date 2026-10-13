using Microsoft.EntityFrameworkCore;
using SkillSwapBackend.Data;
using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext _context;

    public SkillRepository(AppDbContext context)
    {
        _context = context;
    }

    // CREATE
    public async Task<Skill> CreateSkill(Skill skill)
    {
        _context.Skills.Add(skill);

        await _context.SaveChangesAsync();

        return skill;
    }

    // READ
    public async Task<List<Skill>> GetSkillsByUserId(int userId)
    {
        return await _context.Skills
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersBySkillName(string skillName)
    {
        return await _context.Users
            .Where(user => _context.Skills
                .Any(userSkill =>
                    userSkill.UserId == user.Id &&
                    userSkill.SkillName == skillName))
            .ToListAsync();
    }

    // READ ONE
    public async Task<Skill?> GetSkillById(int id)
    {
        return await _context.Skills.FindAsync(id);
    }

    // UPDATE
    public async Task<Skill?> UpdateSkill(int id, Skill skill)
    {
        var existingSkill = await _context.Skills.FindAsync(id);

        if (existingSkill == null)
        {
            return null;
        }

        existingSkill.SkillName = skill.SkillName;
        existingSkill.SkillType = skill.SkillType;
        existingSkill.SkillLevel = skill.SkillLevel;

        await _context.SaveChangesAsync();

        return existingSkill;
    }

    // DELETE
    public async Task<bool> DeleteSkill(int id)
    {
        var skill = await _context.Skills.FindAsync(id);

        if (skill == null)
        {
            return false;
        }

        _context.Skills.Remove(skill);

        await _context.SaveChangesAsync();

        return true;
    }
}