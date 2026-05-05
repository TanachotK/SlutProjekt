namespace SlutProjekt.Models;

public class DailyInstruction
{
    public DateTime Date { get; set; } = DateTime.Today;
    public DateTime SavedAt { get; set; } = DateTime.Now;
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
