using QuizApi.Models;

namespace QuizApi.Data
{
    public static class QuizCategories
    {
        public static List<Category> GetCategories()
        {
            return QuizData.Questions.Select(q => new Category
            {
                Title = q.Key,
                TotalQuestions = q.Value.Count,
                ImgSrc = GetImageForCategory(q.Key)
            }).ToList();
        }

        private static string GetImageForCategory(string category)
        {
            return category.ToLower() switch
            {
                "html" => "https://cdn-icons-png.flaticon.com/512/732/732212.png",
                "css" => "https://cdn-icons-png.flaticon.com/512/732/732190.png",
                "javascript" => "https://cdn-icons-png.flaticon.com/512/5968/5968292.png",
                "typescript" => "https://cdn-icons-png.flaticon.com/512/5968/5968381.png",
                _ => ""
            };
        }
    }
}
