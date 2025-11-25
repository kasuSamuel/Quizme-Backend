using QuizApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizApi.Data
{
    public class QuizCategoriesService
    {
        private readonly QuizDataService _quizService;
        private readonly ILogger<QuizCategoriesService> _logger;

        public QuizCategoriesService(QuizDataService quizService, ILogger<QuizCategoriesService> logger)
        {
            _quizService = quizService;
            _logger = logger;
        }

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

        public void AddCategory(string title, string imgSrc, int defaultTimeLimit)
        {
            // Check if the category already exists before adding it
            var existingCategory = _quizService.GetCategoryObjects().FirstOrDefault(c => c.Title == title);
            if (existingCategory != null)
            {
                _logger.LogError($"Duplicate category title detected: {title}");
                throw new Exception($"Category with title '{title}' already exists.");
            }

            // Proceed with adding the category if it doesn't exist
            _quizService.AddCategory(title, imgSrc, defaultTimeLimit);
        }

        public bool UpdateCategory(int id, string newTitle, string newImgSrc, int defaultTimeLimit)
        {
            // Forward the update request to QuizDataService
            return _quizService.UpdateCategory(id, newTitle, newImgSrc,  defaultTimeLimit);
        }

        public bool DeleteCategory(int id)
        {
            // Forward the delete request to QuizDataService
            return _quizService.DeleteCategory(id);
        }
    }
}
