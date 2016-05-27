using System;
using System.Collections.Generic;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public class Сriminal
    {
        #region Properies
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

        public CriminalStateOptions State { get; set; }
        #endregion

        public Сriminal()
        {
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
            State = CriminalStateOptions.Wanted;
        }
        public Сriminal(string lastname, string forename, string codeName, ushort height, 
            EyeColor colorOfEye, HairColor colorOfHair, SexOptions sex, List<string> specialSigns,
            Country citizenship, Country birthCountry, string birthplace, DateTime birthdate, 
            Country lastLivingCountry, string lastLivingPlace, List<Language> languages)
        {
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
            State = CriminalStateOptions.Wanted;
        }

        #region Enums
        public enum SexOptions
        {
            Male,
            Female,
            Unknown
        }
        public enum CriminalStateOptions
        {
            Wanted,
            Busted,
            Wasted
        }
        #endregion
    }
}
