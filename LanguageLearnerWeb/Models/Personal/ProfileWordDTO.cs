using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageLearnerWeb.Models
{
    public class ProfileWordDTO
    {
        public int Id { get; set; }
        public int WordId { get; set; }

        #region Public Word part
        public int LanguageFromId { get; set; }
        public int LanguageToId { get; set; }
        
        public string Name { get; set; }
        public string Translation { get; set; }
        public string Transcription { get; set; }
        public string Image { get; set; }
        #endregion

        public string ProfileId { get; set; }
        
        public int Progress { get; set; }
        public bool IsLearning { get; set; }
        public string Tags { get; set; }
    }
}