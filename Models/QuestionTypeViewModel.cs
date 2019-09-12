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
        public SelectList QuestionTypes { get; set; }
        public string questionType { get; set; }
        public Question question { get; set; }

        public QuestionType QuestionType { get; set; }
       
        //public List<QuestionType> AvailableQuestion { get; set; }







        // NOTE: Here we use an expression bodied, read-only property
        //       AND the ?. operator
        //       ...good times....
        //public List<SelectListItem> AvailableOption
        //{
        //    get
        //    {
        //        if (AvailableQuestion == null)
        //        {
        //            return null;
        //        }

        //        var apt = AvailableQuestion?.Select(d => new SelectListItem(d.Name, d.QuestionTypeId.ToString())).ToList();
        //        apt.Insert(0, new SelectListItem("Select Question", "Add Question"));

        //        return apt;
        //    }
        //}
    }
}
