using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Tracing;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace AnagramSolver.Dapper
{
    public class DapperRepo
    {
        private readonly string _connectionString;

        public DapperRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void GetAllWords()
        {
            IEnumerable<Word> words;
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                words = connection.Query<Word>("SELECT * FROM Words");
            }
            Console.WriteLine("All words");
            foreach(var word in words)
            {
                Console.WriteLine(word.Value);
            }
        }

        public void Insert(string word, int? categoryId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Words (Value, CategoryId, CreatedAt) VALUES (@Value, @CategoryId, @CreatedAt)",
                    new {Value = word, CategoryId = categoryId, CreatedAt = DateTime.Now});
            }
        }

        public void Update(string toChange, string changeWord) 
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE Words SET Value = @change where Value = @changing",
                    new {change = changeWord, changing = toChange});
            }
        }

        public void Delete(string word) 
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Words WHERE Value = @value",
                    new { value = word});
            }
        }
    }
}
