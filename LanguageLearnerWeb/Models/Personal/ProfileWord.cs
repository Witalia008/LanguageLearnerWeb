using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfileWord
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Word")]
        [Required]
        public int WordId { get; set; }
        public virtual Word Word { get; set; }

        [ForeignKey("Profile")]
        [Required]
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int Progress { get; set; }
        public bool IsLearning { get; set; }
        public string Tags { get; set; }
    }
}