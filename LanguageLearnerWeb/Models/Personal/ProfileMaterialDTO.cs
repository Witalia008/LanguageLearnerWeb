using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfileMaterialDTO
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }

        #region Public Material part
        public int LanguageId { get; set; }
        public int Difficulty { get; set; }
        public string Image { get; set; }
        public string Headline { get; set; }
        public string Text { get; set; }
        public string ShortDescr { get; set; }
        public double Rating { get; set; }
        #endregion

        public string ProfileId { get; set; }
        public bool IsLearnt { get; set; }
    }
}