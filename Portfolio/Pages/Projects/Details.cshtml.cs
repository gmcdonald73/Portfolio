// using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Pages.Projects
{

    public class DetailsModel : PageModel
    {
        private readonly ProjectService _service;
        private readonly IWebHostEnvironment _environment;

        public Project Project { get; set; }
        public string MarkdownHtml { get; set; }

        public DetailsModel(
            ProjectService service,
            IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        public IActionResult OnGet(string slug)
        {
            Project = _service.GetProjects()
                 .FirstOrDefault(p => p.Id == slug)!;

            if (Project == null)
                return NotFound();

            var markdownPath = Path.Combine(
                _environment.WebRootPath,
                "projects",
                $"{slug}.md");

            if (System.IO.File.Exists(markdownPath))
            {
                var markdown = System.IO.File.ReadAllText(markdownPath);

                MarkdownHtml = "argh"; //  Markdown.ToHtml(markdown);
            }
            else
            {
                MarkdownHtml = "<p>Case study coming soon.</p>";
            }

            return Page();
        }
    }
}