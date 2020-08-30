using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace HW4_29QuestionTagRelationships.Data
{
    public class QTRepository
    {
        private readonly string _connectionString;

        public QTRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Question> GetQuestions()
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                return context.Questions.Include(q => q.LikesQuestions)
                    .Include(q => q.QuestionsTags).ThenInclude(qt => qt.Tag)
                    .OrderByDescending(q => q.DatePosted).ToList();
            }
        }

        public Question GetQuestionById(int questionId)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                return context.Questions
                    .Include(q => q.LikesQuestions)
                    .Include(q => q.QuestionsTags).ThenInclude(qt => qt.Tag)
                    .Include(q => q.Answers).ThenInclude(a => a.User)
                    .FirstOrDefault(q => q.Id == questionId);
            }
        }

        public void AddUser(string email, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            User user = new User { Email = email, PasswordHash = hash };
            using (var context = new QuestionTagContext(_connectionString))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public bool IsEmailAvailable(string email)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                bool isAvailable = !context.Users.Any(u => u.Email == email);
                return isAvailable;
            }
        }

        public User Login(string email, string password)
        {
            User user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValid)
            {
                return user;
            }
            return null;
        }

        public User GetUserByEmail(string email)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                return context.Users.FirstOrDefault((u => u.Email == email));
            }
        }

        public void AddQuestion(Question question, List<string> tagList)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                context.Questions.Add(question);
                context.SaveChanges();
                foreach (string tag in tagList)
                {
                    Tag t = GetTagByName(tag);
                    int tagId;
                    if (t == null)
                    {
                        tagId = AddTagAndReturnId(tag);
                    }
                    else
                    {
                        tagId = t.Id;
                    }
                    context.QuestionsTags.Add(new QuestionsTags
                    {
                        QuestionId = question.Id,
                        TagId = tagId
                    });
                }
                context.SaveChanges();
            }
        }

        private Tag GetTagByName(string name)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                return context.Tags.FirstOrDefault(t => t.Name == name);
            }
        }

        private int AddTagAndReturnId(string name)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                var tag = new Tag { Name = name };
                context.Tags.Add(tag);
                context.SaveChanges();
                return tag.Id;
            }
        }

        public void AddAnswer(Answer answer)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                context.Answers.Add(answer);
                context.SaveChanges();
            }
        }

        public List<Answer> GetAnswersForQuestion(int questionId)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                return context.Answers.Where(a => a.QuestionId == questionId).ToList();
            }
        }
        public void LikeQuestion (LikesQuestions lq)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                context.LikesQuestions.Add(new LikesQuestions
                {
                    QuestionId = lq.QuestionId,
                    UserId = lq.UserId
                });
                context.SaveChanges();
            }
        }
        public int GetCurrentLikes(int questionId)
        {
            using (var context = new QuestionTagContext(_connectionString))
            {
                return context.LikesQuestions.Where(lq => lq.QuestionId == questionId).Count();
            }
        }

    }
}
