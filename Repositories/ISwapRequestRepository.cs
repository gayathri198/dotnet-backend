using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public interface ISwapRequestRepository
{
    Task<SwapRequest> CreateAsync(SwapRequest request);

    Task<List<SwapRequest>> GetByUserIdAsync(int userId);

    Task<SwapRequest?> GetByIdAsync(int id);

    Task<SwapRequest?> UpdateStatusAsync(int id, string status);

    Task<bool> DeleteAsync(int id);
}
