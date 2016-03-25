using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolDatabaseProject.Model
{
    /*
        Данные по каждому зарегистрированному преступнику: фамилия, имя, кличка, рост, цвет волос и глаз, особые приметы, гражданство, 
        место и дата рождения, последнее место жительства, знание языков, преступная профессия, последнее дело и так далее. 
        Преступные и мафиозные группировки (данные о подельщиках). 
    */

    class Сriminal
    {
        public string Lastname { get; set; }
        public string Forenames { get; set; }
        public string CodeName { get; set; }
        public ushort Height { get; set; }
        public EyeColor ColorOfEye { get; set; }
        public HairColor ColorOfHair { get; set; }
        public SexOptions Sex { get; set; }
        public List<string> SpecialSigns { get; set; }
        public Country Nationality { get; set; }
        public Country Citizenship { get; set; }
        public string Birthplace { get; set; }
        public DateTime Birthdate { get; set; }
        public string LastLivingPlace { get; set; }
        public List<Language> Languages { get; set; }
        public string CriminalProfession { get; set; }
        public CriminalGroup CriminalGroupMembership { get; set; }

        public enum SexOptions
        {
            Male,
            Female,
            Unknown
        }
        public class Language
        {
            private int id;

            static Language()
            {
                Languages = new Dictionary<int, string>();
            }

            public static Dictionary<int, string> Languages { get; set; }
            public int Id
            {
                get
                {
                    return id;
                }
                set
                {
                    if (Languages.ContainsKey(value))
                        id = value;
                    else throw new ArgumentOutOfRangeException();
                }
            }

        }
        public class Country
        {
            private int id;

            static Country()
            {
                Countries = new Dictionary<int, string>();
                Countries.Add(0, "Unknown");
            }

            public static Dictionary<int, string> Countries { get; set; }
            public int Id
            {
                get
                {
                    return id;
                }
                set
                {
                    if (Countries.ContainsKey(value))
                        id = value;
                    else throw new ArgumentOutOfRangeException();
                }
            }

        }
        public class HairColor
        {
            private int id;

            static HairColor()
            {
                HairColors = new Dictionary<int, string>();
                HairColors.Add(0, "Unknown");
            }

            public static Dictionary<int, string> HairColors { get; set; }
            public int Id
            {
                get
                {
                    return id;
                }
                set
                {
                    if (HairColors.ContainsKey(value))
                        id = value;
                    else throw new ArgumentOutOfRangeException();
                }
            }

        }
        public class EyeColor
        {
            private int id;

            static EyeColor()
            {
                EyeColors = new Dictionary<int, string>();
                EyeColors.Add(0, "Unknown");
            }

            public static Dictionary<int, string> EyeColors { get; set; }
            public int Id
            {
                get
                {
                    return id;
                }
                set
                {
                    if (EyeColors.ContainsKey(value))
                        id = value;
                    else throw new ArgumentOutOfRangeException();
                }
            }

        }
        public class CriminalGroup
        {
            private int id;

            static CriminalGroup()
            {
                CriminalGroups = new Dictionary<int, string>();
                CriminalGroups.Add(0, "Unknown");
            }

            public static Dictionary<int, string> CriminalGroups { get; set; }
            public int Id
            {
                get
                {
                    return id;
                }
                set
                {
                    if (CriminalGroups.ContainsKey(value))
                        id = value;
                    else throw new ArgumentOutOfRangeException();
                }
            }

        }

    }
}
