using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Services;

public class IndexModel : PageModel
{
    private readonly ProjectService _service;

    public List<Project> FeaturedProjects { get; set; } = [];

    public IndexModel(ProjectService service)
    {
        _service = service;
    }

    public void OnGet()
    {
        FeaturedProjects = _service.GetProjects();
    }
}