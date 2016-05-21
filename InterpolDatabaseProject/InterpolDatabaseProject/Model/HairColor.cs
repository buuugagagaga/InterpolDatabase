using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolDatabaseProject.Model
{
    public struct HairColor
    {
        private int _id;

        static HairColor()
        {
            HairColors = new Dictionary<int, string> { { 0, "Unknown" } };
        }
        public HairColor(int id):this()
        {
            Id = id;
        }
        public static Dictionary<int, string> HairColors { get; set; }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (HairColors.ContainsKey(value))
                    _id = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return HairColors[Id];
        }
    }
}
