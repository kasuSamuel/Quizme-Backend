using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;

namespace QuizApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        [HttpGet("categories")]
        public IActionResult GetCategoryList()
        {
            var categories = QuizCategories.GetCategories();
            return Ok(categories);
        }

        [HttpGet("{category}")]
        public IActionResult GetQuestions(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(new { message = "Category is required" });

            var questionList = QuizData.Questions
                .FirstOrDefault(kvp => kvp.Key.Equals(category, StringComparison.OrdinalIgnoreCase))
                .Value;

            if (questionList == null)
                return NotFound(new { message = $"Category '{category}' not found" });

            return Ok(questionList);
        }
    }
}
