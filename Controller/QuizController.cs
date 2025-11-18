using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;

namespace QuizApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        // GET api/quiz → returns all category names
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(QuizData.Questions.Keys);
        }

        // GET api/quiz/html | api/quiz/JavaScript | etc.
        [HttpGet("{category}")]
        public IActionResult GetQuestions(string category)
        {
            if (string.IsNullOrEmpty(category))
                return BadRequest(new { message = "Category is required" });

            // Case-insensitive lookup
            var questionList = QuizData.Questions
                .FirstOrDefault(kvp => kvp.Key.Equals(category, StringComparison.OrdinalIgnoreCase))
                .Value;

            if (questionList == null)
                return NotFound(new { message = $"Category '{category}' not found" });

            return Ok(questionList);
        }
    }
}
