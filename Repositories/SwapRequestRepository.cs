using Microsoft.EntityFrameworkCore;
using SkillSwapBackend.Data;
using SkillSwapBackend.Models;

namespace SkillSwapBackend.Repositories;

public class SwapRequestRepository : ISwapRequestRepository
{
    private readonly AppDbContext _context;

    public SwapRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SwapRequest> CreateAsync(SwapRequest request)
    {
        _context.SwapRequests.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public Task<List<SwapRequest>> GetByUserIdAsync(int userId)
    {
        return _context.SwapRequests
            .AsNoTracking()
            .Include(request => request.Sender)
            .Include(request => request.Receiver)
            .Where(request => request.SenderId == userId || request.ReceiverId == userId)
            .OrderByDescending(request => request.CreatedAt)
            .ToListAsync();
    }

    public Task<SwapRequest?> GetByIdAsync(int id)
    {
        return _context.SwapRequests
            .Include(request => request.Sender)
            .Include(request => request.Receiver)
            .FirstOrDefaultAsync(request => request.Id == id);
    }

    public async Task<SwapRequest?> UpdateStatusAsync(int id, string status)
    {
        var request = await _context.SwapRequests
            .Include(swapRequest => swapRequest.Sender)
            .Include(swapRequest => swapRequest.Receiver)
            .FirstOrDefaultAsync(swapRequest => swapRequest.Id == id);

        if (request == null)
        {
            return null;
        }

        request.Status = status;
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var request = await _context.SwapRequests.FindAsync(id);
        if (request == null)
        {
            return false;
        }

        _context.SwapRequests.Remove(request);
        await _context.SaveChangesAsync();
        return true;
    }
}
