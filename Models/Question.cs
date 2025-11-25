namespace QuizApi.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public List<string>? Options { get; set; }
        public string? Answer { get; set; }

        public int? TimeLimit { get; set; }
    }
}
