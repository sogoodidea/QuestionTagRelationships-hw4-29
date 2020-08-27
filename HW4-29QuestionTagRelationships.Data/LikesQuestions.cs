using System;
using System.Collections.Generic;
using System.Text;

namespace HW4_29QuestionTagRelationships.Data
{
    public class LikesQuestions
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
    }
}
