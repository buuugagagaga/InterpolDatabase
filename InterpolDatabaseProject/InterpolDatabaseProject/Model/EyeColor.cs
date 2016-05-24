using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public struct EyeColor
    {
        private int _id;

        static EyeColor()
        {
            EyeColors = new List<string> { "Unknown" };
            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            using (Stream stream = new FileStream("Storage/AdditionalData/eyecolors.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                EyeColors = (List<string>)xs.Deserialize(stream);
            }
        }

        public EyeColor(int id):this()
        {
            Id = id;
        }

        public static List<string> EyeColors { get; set; }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value>=0 && value < EyeColors.Count)
                    _id = value;
                else throw new ArgumentOutOfRangeException();
            }
        }
        public override string ToString()
        {
            return EyeColors[Id];
        }
    }
}
