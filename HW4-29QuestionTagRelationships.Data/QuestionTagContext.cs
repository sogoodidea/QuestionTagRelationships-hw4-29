using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW4_29QuestionTagRelationships.Data
{
    public class QuestionTagContext : DbContext
    {
        private string _connectionString;

        public QuestionTagContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LikesQuestions> LikesQuestions { get; set; }
        public DbSet<LikesAnswers> LikesAnswers { get; set; }
        public DbSet<QuestionsTags> QuestionsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<LikesQuestions>()
               .HasKey(uq => new { uq.QuestionId, uq.UserId });
            modelBuilder.Entity<LikesQuestions>()
                .HasOne(uq => uq.Question)
                .WithMany(q => q.LikesQuestions)
                .HasForeignKey(uq => uq.QuestionId);
            modelBuilder.Entity<LikesQuestions>()
                .HasOne(uq => uq.User)
                .WithMany(u => u.LikesQuestions)
                .HasForeignKey(uq => uq.UserId);

            modelBuilder.Entity<LikesAnswers>()
               .HasKey(ua => new { ua.AnswerId, ua.UserId });
            modelBuilder.Entity<LikesAnswers>()
                .HasOne(ua => ua.Answer)
                .WithMany(a => a.LikesAnswers)
                .HasForeignKey(ua => ua.AnswerId);
            modelBuilder.Entity<LikesAnswers>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.LikesAnswers)
                .HasForeignKey(ua => ua.UserId);


            modelBuilder.Entity<QuestionsTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });
            modelBuilder.Entity<QuestionsTags>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionsTags)
                .HasForeignKey(qt => qt.QuestionId);
            modelBuilder.Entity<QuestionsTags>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionsTags)
                .HasForeignKey(qt => qt.TagId);
        } 
    }
}
