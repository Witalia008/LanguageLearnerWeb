using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfileLanguage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Language")]
        [Required]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        [ForeignKey("Profile")]
        [Required]
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [ForeignKey("Level")]
        [Required]
        public int LevelId { get; set; }
        public virtual Level Level { get; set; }

        public int Points { get; set; }
        public double DailyProgress { get; set; }
        public bool IsOnLearn { get; set; }
        public bool IsActive { get; set; }
    }
}