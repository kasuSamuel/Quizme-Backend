using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;

namespace QuizApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        // GET api/quiz
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(QuizData.Questions.Keys);
        }

        // GET api/quiz/HTML
        [HttpGet("{category}")]
        public IActionResult GetQuestions(string category)
        {
            string key = category.ToUpper();

            if (!QuizData.Questions.ContainsKey(key))
                return NotFound(new { message = "Category not found" });

            return Ok(QuizData.Questions[key]);
        }
    }
}
