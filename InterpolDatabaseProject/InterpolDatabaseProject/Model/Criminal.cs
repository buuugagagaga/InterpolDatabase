using System;
using System.Collections.Generic;

namespace InterpolDatabaseProject.Model
{
    /*
        Данные по каждому зарегистрированному преступнику: фамилия, имя, кличка, рост, цвет волос и глаз, особые приметы, гражданство, 
        место и дата рождения, последнее место жительства, знание языков, преступная профессия, последнее дело и так далее. 
        Преступные и мафиозные группировки (данные о подельщиках). 
    */

    class Сriminal
    {
        public int Id { set; get; }

        public string Lastname { get; set; }
        public string Forename { get; set; }
        public string CodeName { get; set; }

        public int Height { get; set; }
        public EyeColor ColorOfEye { get; set; }
        public HairColor ColorOfHair { get; set; }
        public SexOptions Sex { get; set; }
        public List<string> SpecialSigns { get; set; }

        public Country Citizenship { get; set; }
        public Country BirthCountry { get; set; }
        public string Birthplace { get; set; }
        public DateTime Birthdate { get; set; }
        public Country LastLivingCountry { get; set; }
        public string LastLivingPlace { get; set; }
        public List<Language> Languages { get; set; }

        public Сriminal()
        {
            Id = -1;
            Lastname = "Unknown";
            Forename = "Unknown";
            CodeName = "Unknown";
            Height = -1;
            ColorOfEye = new EyeColor(0);
            ColorOfHair = new HairColor(0);
            Sex = SexOptions.Unknown;
            SpecialSigns = new List<string>();
            Citizenship = new Country(0);
            BirthCountry = new Country(0);
            Birthplace = "Unknown";
            LastLivingCountry = new Country(0);
            LastLivingPlace = "Unknown";
            Languages = new List<Language>();
        }
        public Сriminal(int id, string lastname, string forename, string codeName, ushort height, EyeColor colorOfEye, HairColor colorOfHair, SexOptions sex, List<string> specialSigns, Country citizenship, Country birthCountry, string birthplace, DateTime birthdate, Country lastLivingCountry, string lastLivingPlace, List<Language> languages, List<Crime> crimes, CriminalGroup criminalGroupMembership)
        {
            Id = id;
            Lastname = lastname;
            Forename = forename;
            CodeName = codeName;
            Height = height;
            ColorOfEye = colorOfEye;
            ColorOfHair = colorOfHair;
            Sex = sex;
            SpecialSigns = specialSigns;
            Citizenship = citizenship;
            BirthCountry = birthCountry;
            Birthplace = birthplace;
            Birthdate = birthdate;
            LastLivingCountry = lastLivingCountry;
            LastLivingPlace = lastLivingPlace;
            Languages = languages;
        }

        public enum SexOptions
        {
            Male,
            Female,
            Unknown
        }
    }
}
