using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfileLanguageDTO
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
        public string NameOriginal { get; set; } 
        public string ShortName { get; set; }
        public string ShortNameCC { get; set; }

        public int LevelId { get; set; }
        public int Points { get; set; }
        public double DailyProgress { get; set; }
        public bool IsOnLearn { get; set; }
        public bool IsActive { get; set; }

        public string ProfileId { get; set; }
    }
}