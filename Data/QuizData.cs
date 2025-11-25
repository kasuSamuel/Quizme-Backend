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
                    Title TEXT NOT NULL UNIQUE COLLATE NOCASE,
                    ImgSrc TEXT
                );

                CREATE TABLE IF NOT EXISTS Questions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CategoryId INTEGER NOT NULL,
                    QuestionText TEXT NOT NULL,
                    Options TEXT,
                    Answer TEXT,
                    TimeLimit INTEGER,
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
            cmd.Parameters.AddWithValue("$imgSrc", imgSrc ?? string.Empty);
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
            return rowsAffected > 0;
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
            return rowsAffected > 0;
        }

        // ---------------- ADD QUESTION ----------------
        public void AddQuestion(string categoryTitle, Question question)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var getCat = connection.CreateCommand();
            // Case-insensitive comparison using LOWER()
            getCat.CommandText = "SELECT Id FROM Categories WHERE LOWER(Title) = LOWER($title)";
            getCat.Parameters.AddWithValue("$title", categoryTitle);

            var categoryId = (long?)getCat.ExecuteScalar();
            if (categoryId == null) return;

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Questions (CategoryId, QuestionText, Options, Answer, TimeLimit)
                VALUES ($categoryId, $text, $options, $answer, $timeLimit)
            ";
            cmd.Parameters.AddWithValue("$categoryId", categoryId);
            cmd.Parameters.AddWithValue("$text", question.QuestionText);
            cmd.Parameters.AddWithValue("$options", question.Options != null ? JsonSerializer.Serialize(question.Options) : null);
            cmd.Parameters.AddWithValue("$answer", question.Answer);
            cmd.Parameters.AddWithValue("$timeLimit", question.TimeLimit);

            cmd.ExecuteNonQuery();
        }

        // ---------------- GET QUESTIONS BY CATEGORY ----------------
        public List<Question> GetQuestionsByCategory(string title)
        {
            var list = new List<Question>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            // Case-insensitive comparison using LOWER()
            cmd.CommandText = @"
                SELECT Questions.Id, Questions.QuestionText, Questions.Options, Questions.Answer, Questions.TimeLimit
                FROM Questions
                JOIN Categories ON Categories.Id = Questions.CategoryId
                WHERE LOWER(Categories.Title) = LOWER($title)
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
                    Answer = reader.IsDBNull(3) ? null : reader.GetString(3),
                    TimeLimit = reader.GetInt32(4)
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
        public bool UpdateQuestion(int questionId, Question updatedQuestion)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Questions
                SET QuestionText = $text,
                    Options = $options,
                    Answer = $answer,
                    TimeLimit = $timeLimit
                WHERE Id = $id
            ";
            cmd.Parameters.AddWithValue("$text", updatedQuestion.QuestionText);
            cmd.Parameters.AddWithValue("$options", updatedQuestion.Options != null ? JsonSerializer.Serialize(updatedQuestion.Options) : null);
            cmd.Parameters.AddWithValue("$answer", updatedQuestion.Answer);
            cmd.Parameters.AddWithValue("$timeLimit", updatedQuestion.TimeLimit);
            cmd.Parameters.AddWithValue("$id", questionId);

            var rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        // ---------------- DELETE QUESTION ----------------
        public bool DeleteQuestion(int questionId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM Questions
                WHERE Id = $id
            ";
            cmd.Parameters.AddWithValue("$id", questionId);

            var rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
}
