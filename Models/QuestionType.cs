using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASK_App.Models
{
    public class QuestionType
    {
        [Key]
        public int QuestionTypeId { get; set; }
        public string Name { get; set; }

    }
}
