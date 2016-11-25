using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class Profile
    {
        [Key, ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserImage { get; set; }

        //[ForeignKey("Language")]
        //[Required]
        //public int LanguageOfInterfaceId;
        //public virtual Language Language { get; set; }
    }
}