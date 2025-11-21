// QuizApi/Data/QuizDataService.cs

using Microsoft.Data.Sqlite;
using QuizApi.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace QuizApi.Data
{
    public class QuizDataService
    {
        private readonly string _connectionString = "Data Source=./quiz.db";

        public QuizDataService()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Categories (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL UNIQUE,
                    ImgSrc TEXT
                );

                CREATE TABLE IF NOT EXISTS Questions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryId INTEGER NOT NULL,
                    QuestionText TEXT NOT NULL,
                    Options TEXT,
                    Answer TEXT,
                    FOREIGN KEY (CategoryId) REFERENCES Categories (Id)
                );
            ";
            cmd.ExecuteNonQuery();
        }

        // ---------------- ADD CATEGORY ----------------
        public void AddCategory(string title, string imgSrc)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Categories (Title, ImgSrc)
                VALUES ($title, $imgSrc)
            ";
            cmd.Parameters.AddWithValue("$title", title);
            cmd.Parameters.AddWithValue("$imgSrc", imgSrc);
            cmd.ExecuteNonQuery();
        }

        // ---------------- UPDATE CATEGORY ----------------
        public bool UpdateCategory(int id, string newTitle, string newImgSrc)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Categories
                SET Title = $newTitle, ImgSrc = $newImgSrc
                WHERE Id = $id
            ";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$newTitle", newTitle);
            cmd.Parameters.AddWithValue("$newImgSrc", newImgSrc ?? string.Empty);

            var rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0; // returns true if category was updated
        }

        // ---------------- DELETE CATEGORY ----------------
        public bool DeleteCategory(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM Categories
                WHERE Id = $id
            ";
            cmd.Parameters.AddWithValue("$id", id);

            var rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0; // returns true if category was deleted
        }

        // ---------------- ADD QUESTION ----------------
        public void AddQuestion(string categoryTitle, Question question)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var getCat = connection.CreateCommand();
            getCat.CommandText = "SELECT Id FROM Categories WHERE Title = $title";
            getCat.Parameters.AddWithValue("$title", categoryTitle);

            var categoryId = (long?)getCat.ExecuteScalar();
            if (categoryId == null) return;

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Questions (CategoryId, QuestionText, Options, Answer)
                VALUES ($categoryId, $text, $options, $answer)
            ";
            cmd.Parameters.AddWithValue("$categoryId", categoryId);
            cmd.Parameters.AddWithValue("$text", question.QuestionText);
            cmd.Parameters.AddWithValue("$options", question.Options != null ? JsonSerializer.Serialize(question.Options) : null);
            cmd.Parameters.AddWithValue("$answer", question.Answer);

            cmd.ExecuteNonQuery();
        }

        // ---------------- GET QUESTIONS BY CATEGORY ----------------
        public List<Question> GetQuestionsByCategory(string title)
        {
            var list = new List<Question>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Questions.Id, Questions.QuestionText, Questions.Options, Questions.Answer
                FROM Questions
                JOIN Categories ON Categories.Id = Questions.CategoryId
                WHERE Categories.Title = $title
            ";
            cmd.Parameters.AddWithValue("$title", title);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Question
                {
                    Id = reader.GetInt32(0),
                    QuestionText = reader.GetString(1),
                    Options = reader.IsDBNull(2) ? null : JsonSerializer.Deserialize<List<string>>(reader.GetString(2)),
                    Answer = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }

            return list;
        }

        // ---------------- GET ALL CATEGORIES ----------------
        public List<Category> GetCategoryObjects()
        {
            var list = new List<Category>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, ImgSrc FROM Categories";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    ImgSrc = reader.IsDBNull(2) ? "" : reader.GetString(2)
                });
            }

            return list;
        }

// ---------------- UPDATE QUESTION ----------------
public bool UpdateQuestion(string categoryTitle, int questionId, Question updatedQuestion)
{
    using var connection = new SqliteConnection(_connectionString);
    connection.Open();

    // Get category ID
    var getCat = connection.CreateCommand();
    getCat.CommandText = "SELECT Id FROM Categories WHERE Title = $title";
    getCat.Parameters.AddWithValue("$title", categoryTitle);
    var categoryId = (long?)getCat.ExecuteScalar();
    if (categoryId == null) return false;

    // Update question directly by Id
    var cmd = connection.CreateCommand();
    cmd.CommandText = @"
        UPDATE Questions
        SET QuestionText = $text,
            Options = $options,
            Answer = $answer
        WHERE Id = $id AND CategoryId = $categoryId
    ";
    cmd.Parameters.AddWithValue("$text", updatedQuestion.QuestionText);
    cmd.Parameters.AddWithValue("$options", updatedQuestion.Options != null ? JsonSerializer.Serialize(updatedQuestion.Options) : null);
    cmd.Parameters.AddWithValue("$answer", updatedQuestion.Answer);
    cmd.Parameters.AddWithValue("$id", questionId);
    cmd.Parameters.AddWithValue("$categoryId", categoryId);

    var rowsAffected = cmd.ExecuteNonQuery();
    return rowsAffected > 0; // returns true if question was updated
}


// ---------------- DELETE QUESTION ----------------
public bool DeleteQuestion(string categoryTitle, int questionId)
{
    using var connection = new SqliteConnection(_connectionString);
    connection.Open();

    // Get category ID
    var getCat = connection.CreateCommand();
    getCat.CommandText = "SELECT Id FROM Categories WHERE Title = $title";
    getCat.Parameters.AddWithValue("$title", categoryTitle);
    var categoryId = (long?)getCat.ExecuteScalar();
    if (categoryId == null) return false;

    // Delete the question directly by Id
    var cmd = connection.CreateCommand();
    cmd.CommandText = @"
        DELETE FROM Questions
        WHERE Id = $id AND CategoryId = $categoryId
    ";
    cmd.Parameters.AddWithValue("$id", questionId);
    cmd.Parameters.AddWithValue("$categoryId", categoryId);

    var rowsAffected = cmd.ExecuteNonQuery();
    return rowsAffected > 0; // returns true if question was deleted
}

    }
}
