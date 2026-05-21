using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SlutProjekt.Models;


namespace SlutProjekt.Pages;
   

public class IndexModel : PageModel
{
    private static readonly object _fileLock = new();
    private static readonly List<DailyInstruction> _instructions = new();
    private static readonly string _dataFilePath = Path.Combine(Environment.CurrentDirectory, "journal-entries.json");
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    static IndexModel()
    {
        LoadEntries();
    }

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

        SaveEntries();
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int index)
    {
        if (index >= 0 && index < _instructions.Count)
        {
            _instructions.RemoveAt(index);
            SaveEntries();
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

    private static void LoadEntries()
    {
        lock (_fileLock)
        {
            if (!System.IO.File.Exists(_dataFilePath))
            {
                return;
            }

            try
            {
                var json = System.IO.File.ReadAllText(_dataFilePath);
                var loaded = JsonSerializer.Deserialize<List<DailyInstruction>>(json, _jsonOptions);
                if (loaded != null)
                {
                    _instructions.Clear();
                    _instructions.AddRange(loaded);
                }
            }
            catch
            {
                // ignore invalid file and keep empty list
            }
        }
    }

    private static void SaveEntries()
    {
        lock (_fileLock)
        {
            try
            {
                var json = JsonSerializer.Serialize(_instructions, _jsonOptions);
                System.IO.File.WriteAllText(_dataFilePath, json);
            }
            catch
            {
                // ignore save errors for now
            }
        }
    }
}

