using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;
using QuizApi.Models;

namespace QuizApi.Controller
{
    [ApiController]
    [Route("api/[controller]", Name = "Quiz")]  // Capital Q for controller
    [Route("api/quiz")]                        // Lowercase fallback for Linux
    [Route("api/Quiz")]                        // Explicit capital for consistency
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
            ["JAVASCORE"] = new CategoryInfo  // ← FIXED: was JAVASCRIPT (too long? assuming typo)
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

        // GET api/quiz or api/Quiz → returns list of categories with metadata
        [HttpGet("", Name = "GetCategories")]  // Empty route for base
        public IActionResult GetCategories()
        {
            var result = QuizData.Questions
                .Select(kvp => new
                {
                    title = CategoryMetadata.ContainsKey(kvp.Key) ? CategoryMetadata[kvp.Key].Title : kvp.Key,
                    questions = kvp.Value.Count,
                    imgSrc = CategoryMetadata.ContainsKey(kvp.Key) ? CategoryMetadata[kvp.Key].ImgSrc : ""
                })
                .ToList();
            return Ok(result);
        }

        // GET api/quiz/HTML or api/Quiz/HTML → returns questions
        [HttpGet("{category}", Name = "GetQuestions")]
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
