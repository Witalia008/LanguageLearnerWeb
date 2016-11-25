using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public string Source { get; set; }

        [Required]
        public string Name { get; set; }
        public string NameOriginal { get; set; }

        [Required]
        public string ShortName { get; set; }
        [RegularExpression(@"^[A-Z]{2}-[A-Z]{2}$")]
        [StringLength(5)]
        [Index(IsUnique = true)]
        public string ShortNameCC { get; set; }
    }
}