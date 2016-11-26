using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class Word
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("LanguageFrom")]
        [Required]
        public int LanguageFromId { get; set; }
        public virtual Language LanguageFrom { get; set; }

        [ForeignKey("LanguageTo")]
        [Required]
        public int LanguageToId { get; set; }
        public virtual Language LanguageTo { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Translation { get; set; }
        public string Transcription { get; set; }
        public string Image { get; set; }
    }
}