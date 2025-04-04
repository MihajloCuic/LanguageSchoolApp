using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class LanguageProficiency
    {
        public string LanguageName { get; set; }
        public LanguageLevel LanguageLevel { get; set; }

        public LanguageProficiency() { }

        public LanguageProficiency(string _languageName, LanguageLevel _languageLevel) { 
            LanguageName = _languageName;
            LanguageLevel = _languageLevel;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not LanguageProficiency other)
            {
                return false;
            }
            return LanguageName == other.LanguageName && LanguageLevel.Equals(other.LanguageLevel);
        }
    }
}
