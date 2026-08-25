using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Services;
using System.Text.RegularExpressions;

namespace Portfolio.Pages.Projects
{

    public class DetailsModel : PageModel
    {
        private readonly ProjectService _service;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IWebHostEnvironment _environment;  // !!! remove this

        public Project? Project { get; set; }
        public string? MarkdownHtml { get; set; }

        public DetailsModel(
            ProjectService service,
            IWebHostEnvironment environment,
            IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _environment = environment;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> OnGetAsync(string slug)
        {
            var project = _service.GetProjects()
                .FirstOrDefault(p => p.Id == slug);

            if (project is null)
            {
                return NotFound();
            }

            Project = project;

            var rawBaseUrl = GetRawBaseUrl(Project);

            var readmeUrl = rawBaseUrl + "/README.md";


            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync(readmeUrl);

                if (response.IsSuccessStatusCode)
                {
                    var markdown = await response.Content.ReadAsStringAsync();
                    markdown = FixImageUrls(markdown, rawBaseUrl);
                    // MarkdownHtml = Markdown.ToHtml(markdown);

                    var pipeline = new MarkdownPipelineBuilder()
                        .UseAdvancedExtensions()
                        .UseAutoLinks()
                        .Build();

                    MarkdownHtml = Markdown.ToHtml(markdown, pipeline);
                }
                else
                {
                    MarkdownHtml = "<p>Project documentation is not currently available.</p>";
                }
            }
            catch (HttpRequestException)
            {
                MarkdownHtml = "<p>Project documentation is temporarily unavailable.</p>";
            }

            return Page();
        }


        private string GetRawBaseUrl(Project project)
        {
            var uri = new Uri(project.GitHubUrl);

            var segments = uri.AbsolutePath
                .Trim('/')
                .Split('/');

            return $"https://raw.githubusercontent.com/" +
                   $"{segments[0]}/{segments[1]}/{project.GitHubBranch}";
        }

        private string FixImageUrls(string markdown, string rawBaseUrl)
        {
            return Regex.Replace(
                markdown,
                @"(!\[[^\]]*\]\()(\.?/?[^)]+)(\))",
                match =>
                {
                    var imagePath = match.Groups[2].Value;

                    // Don't modify absolute URLs
                    if (imagePath.StartsWith("http://") ||
                        imagePath.StartsWith("https://"))
                    {
                        return match.Value;
                    }

                    imagePath = imagePath.TrimStart('.', '/');

                    return $"{match.Groups[1].Value}{rawBaseUrl}/{imagePath}{match.Groups[3].Value}";
                });
        }
    }
}