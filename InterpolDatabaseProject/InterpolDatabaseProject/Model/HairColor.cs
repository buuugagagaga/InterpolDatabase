using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public struct HairColor
    {
        private int _id;

        static HairColor()
        {
            HairColors = new List<string> { "Unknown" };
            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            using (Stream stream = new FileStream("Storage/AdditionalData/haircolors.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                HairColors = (List<string>)xs.Deserialize(stream);
            }
        }
        public HairColor(int id):this()
        {
            Id = id;
        }
        public static List<string> HairColors { get; set; }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value>0 && value<HairColors.Count)
                    _id = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return HairColors[Id];
        }
        public static bool operator ==(HairColor h1, HairColor h2)
        {
            return h1.Id == h2.Id;
        }
        public static bool operator !=(HairColor h1, HairColor h2)
        {
            return h1.Id != h2.Id;
        }
    }
}
