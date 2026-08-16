using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Pages.Projects;

public class IndexModel : PageModel
{
    private readonly ProjectService _service;

    public List<Project> Projects { get; set; } = [];

    public IndexModel(ProjectService service)
    {
        _service = service;
    }

    public void OnGet()
    {
        Projects = _service.GetProjects();
    }
}