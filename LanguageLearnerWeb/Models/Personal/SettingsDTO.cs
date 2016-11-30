using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class SettingsDTO
    {
        public int Id { get; set; }
        public string ProfileId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}