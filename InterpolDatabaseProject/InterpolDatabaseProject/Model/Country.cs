using System;
using System.Collections.Generic;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public struct Country
    {
        private int _id;

        static Country()
        {
            Countries = new Dictionary<int, string> {{0, "Unknown"}};
        }

        public Country(int id):this()
        {
            Id = id;
        }

        public static Dictionary<int, string> Countries { get; set; }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (Countries.ContainsKey(value))
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
