using System;
using System.Collections.Generic;
using System.Text;

namespace HW4_29QuestionTagRelationships.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List <Question> Questions { get; set; }
        public List <Answer> Answers { get; set; }
        public List <LikesQuestions> LikesQuestions { get; set; }
        public List <LikesAnswers> LikesAnswers { get; set; }
    }
}
