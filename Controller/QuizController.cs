// QuizApi/Controller/QuizController.cs
using Microsoft.AspNetCore.Mvc;
using QuizApi.Data;
using QuizApi.Models;
using System.Collections.Generic;
using System;

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

            try
            {
                _categoryService.AddCategory(category.Title, category.ImgSrc ?? "");
                return Ok(new { message = "Category added successfully" });
            }
            catch (Exception ex)
            {
                // Return a BadRequest with the error message if the category already exists
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT api/quiz/categories/{id} → edit category (title or imgSrc)
        [HttpPut("categories/{id}")]
        public IActionResult EditCategory(int id, [FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category?.Title))
                return BadRequest(new { message = "Title is required" });

            var updated = _categoryService.UpdateCategory(id, category.Title, category.ImgSrc);
            return updated
                ? Ok(new { message = "Category updated" })
                : NotFound(new { message = "Category not found" });
        }

        // DELETE api/quiz/categories/{id} → delete category
        [HttpDelete("categories/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var deleted = _categoryService.DeleteCategory(id);
            return deleted
                ? Ok(new { message = "Category deleted" })
                : NotFound(new { message = "Category not found" });
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

 // PUT api/quiz/questions/{id}
[HttpPut("questions/{id}")]
public IActionResult UpdateQuestion(int id, [FromBody] Question question)
{
    return _quizService.UpdateQuestion(id, question)
        ? Ok(new { message = "Question updated" })
        : NotFound(new { message = "Question not found" });
}
// DELETE api/quiz/questions/{id}
[HttpDelete("questions/{id}")]
public IActionResult DeleteQuestion(int id)
{
    return _quizService.DeleteQuestion(id)
        ? Ok(new { message = "Question deleted" })
        : NotFound(new { message = "Question not found" });
}
    }
}
