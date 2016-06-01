using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace InterpolDatabaseProject.Model
{

    #region Enums
    /// <summary>
    /// Пол
    /// </summary>
    public enum SexOptions
    {
        Unknown,
        Male,
        Female
    }

    /// <summary>
    /// Статус преступника
    /// </summary>
    public enum CriminalStateOptions
    {
        Wanted,
        Busted,
        Wasted
    }

    /// <summary>
    /// Виды преступлений
    /// </summary>
    public enum Crime
    {
        Robbery,
        Murder,
        Terrorism
    }

    /// <summary>
    /// Страна
    /// </summary>
    public enum Country
    {
        Unknown,
        USA,
        Ukraine,
        Russia,
        China,
        Columbia
    }

    /// <summary>
    /// Цвет глаз
    /// </summary>
    public enum EyeColor
    {
        Unknown,
        Black,
        Brown,
        Red,
        Gray
    }

    /// <summary>
    /// Цвет волос
    /// </summary>
    public enum HairColor
    {
        Unknown,
        Black,
        Brown,
        Red,
        Gray,
        Bold
    }

    /// <summary>
    /// Язык
    ///  </summary>
    public enum Language
    {
        English,
        Russian,
        Ukrainian,
        Spanish,
        French
    }
    #endregion

    [Serializable]
    public class Сriminal : ISerializable
    {
        #region Fields
        /// <summary>
        /// Отображает последний использованный для преступника ID
        /// </summary>
        public static int LastId { get; private set; } = -1;
        #endregion
        #region Constructors
        /// <summary>
        /// Конструктор для десериализации
        /// </summary>
        public Сriminal(SerializationInfo info, StreamingContext context)
        {
            LastId = info.GetInt32("static._lastId");
            Id = info.GetInt32("Id");
            Lastname = info.GetString("Lastname");
            CodeName = info.GetString("CodeName");
            Forename = info.GetString("Forename");
            Height = (int?)info.GetValue("Height", typeof(int?));
            ColorOfEye = (EyeColor)info.GetValue("ColorOfEye", typeof(EyeColor));
            ColorOfHair = (HairColor)info.GetValue("ColorOfHair", typeof(HairColor));
            Sex = (SexOptions)info.GetValue("Sex", typeof(SexOptions));
            SpecialSigns = (string)info.GetValue("SpecialSigns", typeof(string));
            Citizenship = (Country)info.GetValue("Citizenship", typeof(Country));
            BirthCountry = (Country)info.GetValue("BirthCountry", typeof(Country));
            Birthplace = info.GetString("Birthplace");
            Birthdate = (DateTime?)info.GetValue("Birthdate", typeof(DateTime?));
            LastLivingCountry = (Country)info.GetValue("LastLivingCountry",
                typeof(Country));
            LastLivingPlace = info.GetString("LastLivingPlace");
            Languages = (List<Language>)info.GetValue("Languages", typeof(List<Language>));
            State = (CriminalStateOptions)info.GetValue("State",
                typeof(CriminalStateOptions));
            CriminalGroupMembership = (CriminalGroup)info.GetValue("CriminalGroupMembership",
                typeof(CriminalGroup));
            PhotoFileName = info.GetString("PhotoFileName");
            Charges = (List<Crime>)info.GetValue("Charges", typeof(List<Crime>));
        }

        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="lastname">Фамилия преступника</param>
        /// <param name="forename">Имя преступника</param>
        /// <param name="codeName">Кличка преступника</param>
        /// <param name="height">Рост преступника</param>
        /// <param name="colorOfEye">Цвет глаз преступника</param>
        /// <param name="colorOfHair">Цвет волос преступника</param>
        /// <param name="sex">Пол преступника</param>
        /// <param name="specialSigns">Особые приметы преступника, 
        /// дополнительные данные</param>
        /// <param name="citizenship">Гражданство преступника</param>
        /// <param name="birthCountry">Страна рождения преступника</param>
        /// <param name="birthplace">Место рождения преступника</param>
        /// <param name="birthdate">Дата рождения преступника</param>
        /// <param name="lastLivingCountry">Последняя зарегистрированная страна
        ///  проживания преступника</param>
        /// <param name="lastLivingPlace">Последнее зарегистрированное место
        ///  проживания преступника</param>
        /// <param name="languages">Языки, которыми владеет преступник</param>
        /// <param name="state">Текущее состояние преступника</param>
        /// <param name="photoFileName">Название файла с фотографией преступника</param>
        /// <param name="criminalGroup">Принадлежность преступника к криминальной группировке</param>
        /// <param name="charges">Преступления, в которых обвиняется преступник</param>
        public Сriminal(string lastname, string forename, string codeName, int? height,
            EyeColor colorOfEye, HairColor colorOfHair, SexOptions sex, string specialSigns,
            Country citizenship, Country birthCountry, string birthplace,
            DateTime? birthdate, Country lastLivingCountry, string lastLivingPlace,
            List<Language> languages, CriminalStateOptions state, string photoFileName,
            CriminalGroup criminalGroup, List<Crime> charges)
        {
            Id = ++LastId;
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
            if (criminalGroup != null)
                SetCriminalGroup(criminalGroup);
            PhotoFileName = photoFileName;
            Charges = charges;
        }
        #endregion
        #region Properies

        /// <summary>
        /// Уникальный номер преступника
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Фамилия преступника
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Имя преступника
        /// </summary>
        public string Forename { get; set; }

        /// <summary>
        /// Кличка преступника
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// Рост преступника
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Цвет глаз преступника
        /// </summary>
        public EyeColor ColorOfEye { get; set; }

        /// <summary>
        /// Цвет волос преступника
        /// </summary>
        public HairColor ColorOfHair { get; set; }

        /// <summary>
        /// Пол преступника
        /// </summary>
        public SexOptions Sex { get; set; }

        /// <summary>
        /// Особые признаки, дополнительные данные
        /// </summary>
        public string SpecialSigns { get; set; }

        /// <summary>
        /// Гражданство преступника
        /// </summary>
        public Country Citizenship { get; set; }

        /// <summary>
        /// Страна рождения преступника
        /// </summary>
        public Country BirthCountry { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public string Birthplace { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Последняя зарегистрированная страна проживания
        /// </summary>
        public Country LastLivingCountry { get; set; }

        /// <summary>
        /// Последнее зарегистрированное место проживания
        /// </summary>
        public string LastLivingPlace { get; set; }

        /// <summary>
        /// Список языков, на которых говорит преступник
        /// </summary>
        public List<Language> Languages { get; set; }

        /// <summary>
        /// Текущий статус преступника
        /// </summary>
        public CriminalStateOptions State { get; set; }

        /// <summary>
        /// Членство в преступной группировке
        /// </summary>
        public CriminalGroup CriminalGroupMembership { get; private set; }

        /// <summary>
        /// Имя файла с фотографией преступника
        /// </summary>
        public string PhotoFileName { get; set; }

        /// <summary>
        /// Обвинения в преступлениях
        /// </summary>
        public List<Crime> Charges { get; set; }

        /// <summary>
        /// Текущий возраст преступника
        /// </summary>
        public int? Age
        {
            get
            {
                if (Birthdate == null) return null;
                return DateTime.Today.Year - Birthdate.Value.Year;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Метод для установления членства в преступной группировке.
        /// Необходим для синхронизации коллекции членов группировки 
        /// и свойства CriminalGroupMembership
        /// </summary>
        /// <param name="criminalGroup">Группировка, членом которой 
        /// является преступник</param>
        public void SetCriminalGroup(CriminalGroup criminalGroup)
        {
            if (CriminalGroupMembership != null) CriminalGroupMembership = null;
            if (!criminalGroup.Members.ContainsKey(Id))
                criminalGroup.AddMember(this);
            else CriminalGroupMembership = criminalGroup;
        }

        /// <summary>
        /// Метод для удаления членства в преступной группировке.
        /// Необходим для синхронизации коллекции членов группировки 
        /// и свойства CriminalGroupMembership
        /// </summary>
        public void UnsetCriminalGroup()
        {
            if (CriminalGroupMembership == null) return;
            if (CriminalGroupMembership.Members.ContainsKey(Id))
                CriminalGroupMembership.RemoveMember(Id);
            else CriminalGroupMembership = null;
        }

        /// <summary>
        /// Заполняет SerializationInfo данными, необходимыми для сериализации
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("static._lastId", LastId, typeof(int));
            info.AddValue("Id", Id, typeof(int));

            info.AddValue("Lastname", Lastname, typeof(string));
            info.AddValue("CodeName", CodeName, typeof(string));
            info.AddValue("Forename", Forename, typeof(string));
            info.AddValue("Height", Height, typeof(int?));
            info.AddValue("ColorOfEye", ColorOfEye, typeof(EyeColor));
            info.AddValue("ColorOfHair", ColorOfHair, typeof(HairColor));
            info.AddValue("Sex", Sex, typeof(SexOptions));
            info.AddValue("SpecialSigns", SpecialSigns, typeof(List<string>));
            info.AddValue("Citizenship", Citizenship, typeof(Country));
            info.AddValue("BirthCountry", BirthCountry, typeof(Country));
            info.AddValue("Birthplace", Birthplace, typeof(string));
            info.AddValue("Birthdate", Birthdate, typeof(DateTime?));
            info.AddValue("LastLivingCountry", LastLivingCountry, typeof(Country));
            info.AddValue("LastLivingPlace", LastLivingPlace, typeof(string));
            info.AddValue("Languages", Languages, typeof(List<Language>));
            info.AddValue("State", State, typeof(CriminalStateOptions));
            info.AddValue("CriminalGroupMembership", CriminalGroupMembership,
                typeof(CriminalGroup));
            info.AddValue("PhotoFileName", PhotoFileName, typeof(string));
            info.AddValue("Charges", Charges, typeof(List<Crime>));
        }
        #endregion
    }
}
