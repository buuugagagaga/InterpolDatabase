using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public class Сriminal : ISerializable
    {
        #region Properies
        private static int _lastId = -1;

        public int Id { get; set; }
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

        private Dictionary<int, CriminalGroup> _criminalGroups = new Dictionary<int, CriminalGroup>();
        private Dictionary<int, Crime> _crimes = new Dictionary<int, Crime>();

        public List<string> PhotosList { get; set; }
        public ReadOnlyDictionary<int, CriminalGroup> CriminalGroups => new ReadOnlyDictionary<int, CriminalGroup>(_criminalGroups);
        public ReadOnlyDictionary<int, Crime> Crimes => new ReadOnlyDictionary<int, Crime>(_crimes);

        public int Age => (Birthdate==default(DateTime))?0:DateTime.Now.Year - Birthdate.Year;

        #endregion

        public void AddCrime(Crime crime) => _crimes.Add(crime.Id, crime);
        
        public void AddCriminalGroup(CriminalGroup criminalGroup) => _criminalGroups.Add(criminalGroup.Id, criminalGroup);
    
        public void DeleteCrime(int id) => _crimes.Remove(id);
        
        public void DeleteCriminalGroup(int id) => _criminalGroups.Remove(id);


        public Сriminal(SerializationInfo info, StreamingContext context)
        {
            _lastId = info.GetInt32("static._lastId");
            Id = info.GetInt32("Id");

            Lastname = info.GetString("Lastname");
            CodeName = info.GetString("CodeName");
            Forename = info.GetString("Forename");

            Height = info.GetInt32("Height");

            ColorOfEye = (EyeColor)info.GetValue("ColorOfEye", typeof(EyeColor));
            ColorOfHair = (HairColor) info.GetValue("ColorOfHair", typeof(HairColor));
            Sex = (SexOptions)info.GetValue("Sex", typeof(SexOptions));

            SpecialSigns = (List<string>) info.GetValue("SpecialSigns", typeof(List<string>));
            Citizenship = (Country) info.GetValue("Citizenship", typeof(Country));
            BirthCountry = (Country)info.GetValue("BirthCountry", typeof(Country));
            Birthplace = info.GetString("Birthplace");
            Birthdate = info.GetDateTime("Birthdate");
            LastLivingCountry = (Country)info.GetValue("LastLivingCountry", typeof(Country));
            LastLivingPlace = info.GetString("LastLivingPlace");
            Languages = (List<Language>) info.GetValue("Languages", typeof(List<Language>));
            State = (CriminalStateOptions) info.GetValue("State", typeof(CriminalStateOptions));
            _crimes = (Dictionary<int, Crime>)info.GetValue("_crimes", typeof(Dictionary<int, Crime>));
            _criminalGroups = (Dictionary<int, CriminalGroup>)info.GetValue("_criminalGroups", typeof(Dictionary<int, CriminalGroup>));
            PhotosList = (List<string>) info.GetValue("PhotosList", typeof(List<string>));
        }
        public Сriminal(string lastname, string forename, string codeName, ushort height, 
            EyeColor colorOfEye, HairColor colorOfHair, SexOptions sex, List<string> specialSigns,
            Country citizenship, Country birthCountry, string birthplace, DateTime birthdate, 
            Country lastLivingCountry, string lastLivingPlace, List<Language> languages, CriminalStateOptions state)
        {
            Id = ++_lastId;
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
            State = state;
            _criminalGroups = new Dictionary<int, CriminalGroup>();
            _crimes = new Dictionary<int, Crime>();
            PhotosList = new List<string>();
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("static._lastId", _lastId, typeof(int));
            info.AddValue("Id", Id, typeof(int));

            info.AddValue("Lastname", Lastname, typeof(string));
            info.AddValue("CodeName", CodeName, typeof(string));
            info.AddValue("Forename", Forename, typeof(string));
            info.AddValue("Height", Height, typeof(int));
            info.AddValue("ColorOfEye", ColorOfEye, typeof(EyeColor));
            info.AddValue("ColorOfHair", ColorOfHair, typeof(HairColor));
            info.AddValue("Sex", Sex, typeof(SexOptions));
            info.AddValue("SpecialSigns", SpecialSigns, typeof(List<string>));
            info.AddValue("Citizenship", Citizenship, typeof(Country));
            info.AddValue("BirthCountry", BirthCountry, typeof(Country));
            info.AddValue("Birthplace", Birthplace, typeof(string));
            info.AddValue("Birthdate", Birthdate, typeof(DateTime));
            info.AddValue("LastLivingCountry", LastLivingCountry, typeof(Country));
            info.AddValue("LastLivingPlace", LastLivingPlace, typeof(string));
            info.AddValue("Languages", Languages, typeof(List<Language>));
            info.AddValue("State", State, typeof(CriminalStateOptions));
            info.AddValue("_crimes", _crimes, typeof(Dictionary<int, Crime>));
            info.AddValue("_criminalGroups", _criminalGroups, typeof(Dictionary<int, CriminalGroup>));
            info.AddValue("PhotosList", PhotosList, typeof(List<string>));
        }
    }
}
