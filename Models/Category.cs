namespace QuizApi.Models
{
public class Category
{
    public int Id { get; set; }        // matches DB primary key
    public string Title { get; set; }
    public string ImgSrc { get; set; }
    public int TotalQuestions { get; set; } // optional, can calculate later
}
}
