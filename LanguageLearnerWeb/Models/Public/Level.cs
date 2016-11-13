using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class Level
    {
        public int Id { get; set; }

        [Required]
        public int PointsRequired { get; set; }
        [Required]
        public string Title { get; set; }
    }
}