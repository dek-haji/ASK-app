using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASK_App.Models.QuestionAnswersViewModel
{
    public class AnswerCreateViewModel
    {
        public List<SelectListItem> Questions { get; set; }
        public Answer Answer { get; set; }

        private readonly string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
        public AnswerCreateViewModel() { }

        public AnswerCreateViewModel(string connectionString)
        {
            _connectionString = connectionString;

            Questions = GetAllQuestions()
                .Select(Question => new SelectListItem
                {
                    Text = Question.Name,
                    Value = Question.QuestionId.ToString()
                })
                .ToList();

            Questions.Insert(0, new SelectListItem
            {
                Text = "Choose question...",
                Value = "0"
            });
        }
        private List<Question> GetAllQuestions()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT QuestionId, Name FROM Question";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Question> questions = new List<Question>();
                    while (reader.Read())
                    {
                        questions.Add(new Question
                        {
                            QuestionId = reader.GetInt32(reader.GetOrdinal("QuestionId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }

                    reader.Close();

                    return questions;
                }
            }
        }
    }
}