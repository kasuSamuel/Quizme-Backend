using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;
using QuizApi.Models;

namespace QuizApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private static readonly Dictionary<string, CategoryInfo> CategoryMetadata = new()
        {
            ["HTML"] = new CategoryInfo
            {
                Title = "HTML",
                ImgSrc = "https://cdn-icons-png.flaticon.com/512/732/732212.png"
            },
            ["CSS"] = new CategoryInfo
            {
                Title = "CSS",
                ImgSrc = "https://cdn-icons-png.flaticon.com/512/732/732190.png"
            },
            ["JAVASCRIPT"] = new CategoryInfo
            {
                Title = "JavaScript",
                ImgSrc = "https://cdn-icons-png.flaticon.com/512/5968/5968292.png"
            },
            ["TYPESCRIPT"] = new CategoryInfo
            {
                Title = "TypeScript",
                ImgSrc = "https://cdn-icons-png.flaticon.com/512/5968/5968381.png"
            }
        };

        // GET api/quiz  → returns list of categories with metadata
        [HttpGet]
        public IActionResult GetCategories()
        {
            var result = QuizData.Questions
                .Select(kvp => new
                {
                    title = CategoryMetadata[kvp.Key].Title,
                    questions = kvp.Value.Count,
                    imgSrc = CategoryMetadata[kvp.Key].ImgSrc
                })
                .ToList();

            return Ok(result);
        }

        // GET api/quiz/HTML  → still returns the questions
        [HttpGet("{category}")]
        public IActionResult GetQuestions(string category)
        {
            string key = category.ToUpper();

            if (!QuizData.Questions.ContainsKey(key))
                return NotFound(new { message = "Category not found" });

            return Ok(QuizData.Questions[key]);
        }
    }

    // Helper class for metadata
    public class CategoryInfo
    {
        public string Title { get; set; } = string.Empty;
        public string ImgSrc { get; set; } = string.Empty;
    }
}
