using LanguageSchoolApp.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchoolApp.model.Courses
{
    public class LanguageProficiencySlot : ObservableObject
    {
        private bool _hasData;
        private LanguageProficiency? _proficiency;

        public bool HasData 
        { 
            get { return _hasData; } 
            set 
            { 
                _hasData = value;
                OnPropertyChanged();
            } 
        }

        public LanguageProficiency? Proficiency
        { 
            get { return _proficiency; }
            set 
            {
                _proficiency = value;
                OnPropertyChanged();
            }
        }
    }
}
