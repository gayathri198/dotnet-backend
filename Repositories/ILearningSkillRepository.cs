using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public interface ILearningSkillRepository
{
    Task<LearningSkill> CreateLearningSkill(LearningSkill learningSkill);

    Task<List<LearningSkill>> GetLearningSkillsByUserId(int userId);

    Task<LearningSkill?> GetLearningSkillById(int id);

    Task<LearningSkill?> UpdateLearningSkill(int id, LearningSkill learningSkill);

    Task<bool> DeleteLearningSkill(int id);
}
