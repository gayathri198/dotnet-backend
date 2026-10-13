namespace SkillSwapBackend.ViewModels;

public class SkillViewModel
{
    public int UserId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public string SkillType { get; set; } = string.Empty;

    public string SkillLevel { get; set; } = string.Empty;
}