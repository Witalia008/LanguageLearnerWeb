using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfileMaterial
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Material")]
        [Required]
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

        [ForeignKey("Profile")]
        [Required]
        public string ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public bool IsLearnt { get; set; }
        public double Rating { get; set; }
    }
}