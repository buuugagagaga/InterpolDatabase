using System;
using System.Collections.Generic;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public struct Language
    {
        private int _id;

        static Language()
        {
            Languages = new Dictionary<int, string> { { 0, "Unknown" } };
        }
        public Language(int id):this()
        {
            Id = id;
        }
        public static Dictionary<int, string> Languages { get; set; }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (Languages.ContainsKey(value))
                    _id = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return Languages[Id];
        }
    }
}
