using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolDatabaseProject.Model
{
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
