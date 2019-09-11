using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASK_App.Models
{
    public class QuestionTypeViewModel
    {
        public List<Question> Questions { get; set; }
        public SelectList QuestionTypes{ get; set; }
        public string questionType { get; set; }
        public Question question { get; set; }
        public string SearchString { get; set; }
    }
}
