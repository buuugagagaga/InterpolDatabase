using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public struct Crime
    {
        private int _id;

        static Crime()
        {
            Crimes = new List<string> { "Unknown" };
            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            using (Stream stream = new FileStream("..\\..\\Storage\\AdditionalData\\crimes.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Crimes = (List<string>)xs.Deserialize(stream);
            }
        }
        public Crime(int id) : this()
        {
            Id = id;
        }
        public static List<string> Crimes { get; set; }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value >= 0 && value < Crimes.Count)
                    _id = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return Crimes[Id];
        }
    }
}
