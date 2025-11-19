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
        /// Just forwards to QuizDataService (keeps controller clean)
        /// </summary>
        public void AddCategory(string title, string imgSrc)
        {
            _quizService.AddCategory(title, imgSrc);
        }
    }
}
