using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class Preposition
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Language")]
        [Required]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        [Required]
        public string Prefix { get; set; }
        [Required]
        public string Suffix { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public string Options { get; set; }
        public string Rule { get; set; }
    }
}