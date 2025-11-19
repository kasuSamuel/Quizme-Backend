// QuizApi/Data/QuizCategoriesService.cs
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
            // Check if the category already exists before adding it
            var existingCategory = _quizService.GetCategoryObjects().FirstOrDefault(c => c.Title == title);
            if (existingCategory != null)
            {
                _logger.LogError($"Duplicate category title detected: {title}");
                throw new Exception($"Category with title '{title}' already exists.");
            }

            // Proceed with adding the category if it doesn't exist
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
