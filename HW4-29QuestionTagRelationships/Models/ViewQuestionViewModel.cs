using HW4_29QuestionTagRelationships.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW4_29QuestionTagRelationships.Models
{
    public class ViewQuestionViewModel
    {
        public bool IsAuthenticated { get; set; }
        public Question Question { get; set; }
        public User User { get; set; }
    }
}
