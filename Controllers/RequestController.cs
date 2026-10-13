using Microsoft.AspNetCore.Mvc;
using SkillSwapBackend.Models;
using SkillSwapBackend.Repositories;
using SkillSwapBackend.ViewModels;

namespace SkillSwapBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController : ControllerBase
{
    private static readonly HashSet<string> AllowedStatusChanges =
        new(StringComparer.OrdinalIgnoreCase) { "Accepted", "Declined" };

    private readonly ISwapRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;

    public RequestController(
        ISwapRequestRepository requestRepository,
        IUserRepository userRepository)
    {
        _requestRepository = requestRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest(CreateSwapRequestViewModel model)
    {
        if (model.SenderId == model.ReceiverId)
        {
            return BadRequest("A user cannot send a request to themselves.");
        }

        var sender = await _userRepository.GetUserById(model.SenderId);
        if (sender == null)
        {
            return BadRequest("Sender not found.");
        }

        var receiver = await _userRepository.GetUserById(model.ReceiverId);
        if (receiver == null)
        {
            return BadRequest("Receiver not found.");
        }

        var createdRequest = await _requestRepository.CreateAsync(new SwapRequest
        {
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            SkillOffered = model.SkillOffered.Trim(),
            SkillWanted = model.SkillWanted.Trim(),
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        });

        return CreatedAtAction(
            nameof(GetRequestById),
            new { id = createdRequest.Id },
            ToViewModel(createdRequest, sender, receiver));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRequestById(int id)
    {
        var request = await _requestRepository.GetByIdAsync(id);
        if (request == null)
        {
            return NotFound("Request not found.");
        }

        return Ok(ToViewModel(request, request.Sender!, request.Receiver!));
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetMyRequests(int userId)
    {
        if (await _userRepository.GetUserById(userId) == null)
        {
            return NotFound("User not found.");
        }

        var requests = await _requestRepository.GetByUserIdAsync(userId);
        return Ok(requests.Select(request => ToViewModel(
            request,
            request.Sender!,
            request.Receiver!)));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateSwapRequestStatusViewModel model)
    {
        var status = AllowedStatusChanges.FirstOrDefault(
            allowedStatus => string.Equals(
                allowedStatus,
                model.Status,
                StringComparison.OrdinalIgnoreCase));

        if (status == null)
        {
            return BadRequest("Status must be Accepted or Declined.");
        }

        var request = await _requestRepository.UpdateStatusAsync(id, status);
        if (request == null)
        {
            return NotFound("Request not found.");
        }

        return Ok(ToViewModel(request, request.Sender!, request.Receiver!));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        if (!await _requestRepository.DeleteAsync(id))
        {
            return NotFound("Request not found.");
        }

        return NoContent();
    }

    private static SwapRequestViewModel ToViewModel(
        SwapRequest request,
        User sender,
        User receiver)
    {
        return new SwapRequestViewModel
        {
            Id = request.Id,
            SenderId = request.SenderId,
            SenderName = sender.FullName,
            ReceiverId = request.ReceiverId,
            ReceiverName = receiver.FullName,
            SkillOffered = request.SkillOffered,
            SkillWanted = request.SkillWanted,
            Status = request.Status,
            CreatedAt = request.CreatedAt
        };
    }
}
