using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public struct Language
    {
        private int _id;

        static Language()
        {
            Languages = new List<string> { "Unknown" };
            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            using (Stream stream = new FileStream("..\\..\\Storage\\AdditionalData\\languages.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Languages = (List<string>)xs.Deserialize(stream);
            }
        }
        public Language(int id):this()
        {
            Id = id;
        }
        public static List<string> Languages { get; set; }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value >= 0 && value < Languages.Count)
                    _id = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return Languages[Id];
        }

        public static bool operator ==(Language l1, Language l2)
        {
            return l1.Id == l2.Id;
        }
        public static bool operator !=(Language l1, Language l2)
        {
            return l1.Id != l2.Id;
        }
    }
}
