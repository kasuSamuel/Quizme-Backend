// QuizApi/Controller/QuizController.cs
using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;
using QuizApi.Models;

namespace QuizApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizDataService _quizService;
        private readonly QuizCategoriesService _categoryService;

        public QuizController(QuizDataService quizService, QuizCategoriesService categoryService)
        {
            _quizService = quizService;
            _categoryService = categoryService;
        }

        // GET api/quiz/categories → returns list with correct TotalQuestions
        [HttpGet("categories")]
        public ActionResult<List<Category>> GetCategories()
        {
            return Ok(_categoryService.GetCategories());
        }

        // POST api/quiz/categories
        [HttpPost("categories")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category?.Title))
                return BadRequest(new { message = "Title is required" });

            _categoryService.AddCategory(category.Title, category.ImgSrc ?? "");
            return Ok(new { message = "Category added" });
        }

        // GET api/quiz/{category} → e.g. /api/quiz/python
        [HttpGet("{category}")]
        public ActionResult<List<Question>> GetQuestions(string category)
        {
            var questions = _quizService.GetQuestionsByCategory(category);
            return questions.Count > 0
                ? Ok(questions)
                : NotFound(new { message = $"No questions found for '{category}'" });
        }

        // POST api/quiz/{category}/questions
        [HttpPost("{category}/questions")]
        public IActionResult AddQuestion(string category, [FromBody] Question question)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(new { message = "Category is required in URL" });

            if (question == null || string.IsNullOrWhiteSpace(question.QuestionText))
                return BadRequest(new { message = "Valid question is required" });

            _quizService.AddQuestion(category, question);
            return Ok(new { message = "Question added successfully" });
        }

        // PUT api/quiz/{category}/questions/{index}
        [HttpPut("{category}/questions/{index}")]
        public IActionResult UpdateQuestion(string category, int index, [FromBody] Question question)
        {
            return _quizService.UpdateQuestion(category, index, question)
                ? Ok(new { message = "Question updated" })
                : NotFound(new { message = "Question not found" });
        }

        // DELETE api/quiz/{category}/questions/{index}
        [HttpDelete("{category}/questions/{index}")]
        public IActionResult DeleteQuestion(string category, int index)
        {
            return _quizService.DeleteQuestion(category, index)
                ? Ok(new { message = "Question deleted" })
                : NotFound(new { message = "Question not found" });
        }
    }
}
