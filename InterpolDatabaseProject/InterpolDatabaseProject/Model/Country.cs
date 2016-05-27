using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public struct Country
    {
        private int _id;

        static Country()
        {
            Countries = new List<string> { "Unknown" };
            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            using (Stream stream = new FileStream("..\\..\\Storage\\AdditionalData\\countries.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Countries = (List<string>) xs.Deserialize(stream);
            }
        }

        public Country(int id):this()
        {
            Id = id;
        }

        public static List<string> Countries { get; set; }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value>=0 && value < Countries.Count)
                    _id = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return Countries[Id];
        }
    }
}
