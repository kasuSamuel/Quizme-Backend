namespace QuizApi.Models
{
public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ImgSrc { get; set; }
    public int TotalQuestions { get; set; }
    public int DefaultTimeLimit { get; set; }

}
}
