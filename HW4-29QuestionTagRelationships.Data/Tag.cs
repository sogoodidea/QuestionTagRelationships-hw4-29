using System;
using System.Collections.Generic;
using System.Text;

namespace HW4_29QuestionTagRelationships.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestionsTags> QuestionsTags { get; set; }
    }
}
