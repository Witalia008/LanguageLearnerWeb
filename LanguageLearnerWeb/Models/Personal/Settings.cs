using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Profile")]
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}