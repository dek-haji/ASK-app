using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASK_App.Models
{
    public class Question
    {
        
        public int QuestionId { get; set; }
        [Required]
        [Display(Name = "Question")]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Display(Name = "Question Category")]
        public QuestionType QuestionType { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
