using System;
using System.Collections.Generic;
using System.Text;

namespace HW4_29QuestionTagRelationships.Data
{
    public class LikesAnswers
    {
        public int UserId { get; set; }
        public int AnswerId { get; set; }
        public User User { get; set; }
        public Answer Answer { get; set; }
    }
}
