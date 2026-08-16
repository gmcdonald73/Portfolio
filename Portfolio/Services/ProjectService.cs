using System.Text.Json;
using Portfolio.Models;

namespace Portfolio.Services;

public class ProjectService
{
    private readonly IWebHostEnvironment _environment;

    public ProjectService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public List<Project> GetProjects()
    {
        var path = Path.Combine(_environment.ContentRootPath,
            "Data",
            "projects.json");

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<Project>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })
            ?? [];
    }

    public Project? GetProject(string id)
    {
        return GetProjects()
            .FirstOrDefault(x => x.Id == id);
    }
}