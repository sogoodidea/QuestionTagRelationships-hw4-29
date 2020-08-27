using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HW4_29QuestionTagRelationships.Data
{
    public class QuestionTagContextFactory : IDesignTimeDbContextFactory<QuestionTagContext>
    {
        public QuestionTagContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}HW4-29QuestionTagRelationships"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new QuestionTagContext(config.GetConnectionString("ConStr"));
        }
    }
}
