using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Language")]
        [Required]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public int Difficulty { get; set; }
        public string Image { get; set; }
        [Required]
        public string Headline { get; set; }
        [Required]
        public string Text { get; set; }
        public string ShortDescr { get; set; }
        public double Rating { get; set; }
    }
}