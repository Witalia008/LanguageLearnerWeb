using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfilePrepositionDTO
    {
        public int Id { get; set; }

        public int PrepositionId { get; set; }
        public int LanguageId { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Answer { get; set; }
        public string Options { get; set; }
        public string Rule { get; set; }

        public string ProfileId { get; set; }
        
        public int TimesSeen { get; set; }
    }
}