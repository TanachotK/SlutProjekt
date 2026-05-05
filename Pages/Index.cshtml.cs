using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SlutProjekt.Models;

namespace SlutProjekt;

public class IndexModel : PageModel
{
    private static readonly List<DailyInstruction> _instructions = new();

    [BindProperty]
    public DailyInstruction Instruction { get; set; } = new();

    [BindProperty]
    public int? EditIndex { get; set; }

    public bool IsEditing => EditIndex.HasValue;

    public IReadOnlyList<DailyInstruction> Instructions => _instructions;

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (EditIndex.HasValue && EditIndex.Value >= 0 && EditIndex.Value < _instructions.Count)
        {
            var existing = _instructions[EditIndex.Value];
            existing.Date = Instruction.Date;
            existing.SavedAt = DateTime.Now;
            existing.Topic = Instruction.Topic;
            existing.Content = Instruction.Content;
            existing.Goal = Instruction.Goal;
            existing.Notes = Instruction.Notes;
        }
        else
        {
            _instructions.Add(new DailyInstruction
            {
                Date = Instruction.Date,
                SavedAt = DateTime.Now,
                Topic = Instruction.Topic,
                Content = Instruction.Content,
                Goal = Instruction.Goal,
                Notes = Instruction.Notes,
            });
        }

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int index)
    {
        if (index >= 0 && index < _instructions.Count)
        {
            _instructions.RemoveAt(index);
        }

        return RedirectToPage();
    }

    public IActionResult OnPostEdit(int index)
    {
        if (index >= 0 && index < _instructions.Count)
        {
            var item = _instructions[index];
            Instruction = new DailyInstruction
            {
                Date = item.Date,
                SavedAt = item.SavedAt,
                Topic = item.Topic,
                Content = item.Content,
                Goal = item.Goal,
                Notes = item.Notes,
            };

            EditIndex = index;
        }

        return Page();
    }

    public IActionResult OnPostCancel()
    {
        return RedirectToPage();
    }
}

