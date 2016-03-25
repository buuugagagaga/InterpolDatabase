using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolDatabaseProject.Model
{
    class Сriminal
    {
        private ushort eyeColorId;
        private ushort hairColorId;
        private ushort nationalityId;
        private ushort citizenshipId;

        static Сriminal()
        {
            EyeColors = new Dictionary<int, string>();
            EyeColors.Add(0, "Unknown");
            HairColors = new Dictionary<int, string>();
            HairColors.Add(0, "Unknown");
            Countries = new Dictionary<int, string>();
            Countries.Add(0, "Unknown");

        }

        public static Dictionary<int, string> EyeColors { get; set; }
        public static Dictionary<int, string> HairColors { get; set; }
        public static Dictionary<int, string> Countries { get; set; }

        public string Lastname { get; set; }
        public string Forenames { get; set; }
        public string CodeName { get; set; }
        public ushort Height { get; set; }
        public ushort EyeColorId
        {
            get
            {
                return eyeColorId;
            }
            set
            {
                if (EyeColors.ContainsKey(value))
                    eyeColorId = value;
                else throw new ArgumentOutOfRangeException();
            }
        }
        public ushort HairColorId
        {
            get
            {
                return hairColorId;
            }
            set
            {
                if (HairColors.ContainsKey(value))
                    hairColorId = value;
                else throw new ArgumentOutOfRangeException();
            }
        }
        public SexOptions Sex { get; set; }
        public List<string> SpecialSigns { get; set; }
        public ushort NationalityId
        {
            get
            {
                return nationalityId;
            }
            set
            {
                if (Countries.ContainsKey(value))
                    nationalityId = value;
                else throw new ArgumentOutOfRangeException();

            }
        }
        public ushort CitizenshipId
        {
            get
            {
                return citizenshipId;
            }
            set
            {
                if (Countries.ContainsKey(value))
                    citizenshipId = value;
                else throw new ArgumentOutOfRangeException();

            }
        }
        public string Birthplace { get; set; }
        public DateTime Birthdate { get; set; }
        public string LastLivingPlace { get; set; }
        public List<Language> Languages { set; get; }

        public enum SexOptions
        {
            Male,
            Female,
            Unknown
        }

        public class Language
        {
            private int languageId;

            static Language()
            {
                Languages = new Dictionary<int, string>();
                Languages.Add(0, "English");
            }

            public static Dictionary<int, string> Languages { get; set; }
            public int LanguageId
            {
                get
                {
                    return languageId;
                }
                set
                {
                    if (Languages.ContainsKey(value))
                        languageId = value;
                    else throw new ArgumentOutOfRangeException();
                }
            }
            public LanguageKnowledgeLevel KnowledgeLevel { get; set; }
            
            public enum LanguageKnowledgeLevel
            {
                Fluent,
                Middle,
                Low
            }
        }

    }
}
