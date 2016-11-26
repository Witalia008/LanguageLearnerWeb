using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfilePreposition
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Preposition")]
        [Required]
        public int PrepositionId { get; set; }
        public virtual Preposition Preposition { get; set; }

        [ForeignKey("Profile")]
        [Required]
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public int TimesSeen { get; set; }
    }
}