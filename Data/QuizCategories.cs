// QuizApi/Data/QuizCategoriesService.cs
using QuizApi.Models;
using System.Collections.Generic;

namespace QuizApi.Data
{
    public class QuizCategoriesService
    {
        private readonly QuizDataService _quizService;

        public QuizCategoriesService(QuizDataService quizService)
        {
            _quizService = quizService;
        }

        /// <summary>
        /// Returns all categories with correct TotalQuestions count
        /// </summary>
        public List<Category> GetCategories()
        {
            var categories = _quizService.GetCategoryObjects();

            foreach (var cat in categories)
            {
                cat.TotalQuestions = _quizService
                    .GetQuestionsByCategory(cat.Title)
                    .Count;
            }

            return categories;
        }

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        public void AddCategory(string title, string imgSrc)
        {
            _quizService.AddCategory(title, imgSrc);
        }

        /// <summary>
        /// Updates an existing category's details.
        /// </summary>
        public bool UpdateCategory(int id, string newTitle, string newImgSrc)
        {
            // Forward the update request to QuizDataService
            return _quizService.UpdateCategory(id, newTitle, newImgSrc);
        }

        /// <summary>
        /// Deletes a category and all associated questions.
        /// </summary>
        public bool DeleteCategory(int id)
        {
            // Forward the delete request to QuizDataService
            return _quizService.DeleteCategory(id);
        }
    }
}
