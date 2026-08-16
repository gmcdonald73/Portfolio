namespace Portfolio.Models;

public class Project
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public string Image { get; set; } = "";

    public string GitHubUrl { get; set; } = "";

    public string LiveUrl { get; set; } = "";

    public List<string> Technologies { get; set; } = [];

    public List<string> Features { get; set; } = [];
}