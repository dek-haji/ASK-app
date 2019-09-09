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
        public Question Question { get; set; }
        public Answer Answer { get; set; }

    }
}