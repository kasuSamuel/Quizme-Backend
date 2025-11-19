namespace QuizApi.Models
{
    public class Question
    {
        public int Id { get; set; }               // Unique identifier
        public string? QuestionText { get; set; } // Matches DB column
        public List<string>? Options { get; set; }
        public string? Answer { get; set; }
    }
}
