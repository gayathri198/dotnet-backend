using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public interface ISkillRepository
{
    Task<Skill> CreateSkill(Skill skill);

    Task<List<Skill>> GetSkillsByUserId(int userId);

    Task<List<User>> GetUsersBySkillName(string skillName);

    Task<Skill?> GetSkillById(int id);

    Task<Skill?> UpdateSkill(int id, Skill skill);

    Task<bool> DeleteSkill(int id);
}