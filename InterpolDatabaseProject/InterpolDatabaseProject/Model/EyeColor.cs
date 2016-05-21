﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolDatabaseProject.Model
{
    public struct EyeColor
    {
        private int _id;

        static EyeColor()
        {
            EyeColors = new Dictionary<int, string> { { 0, "Unknown" } };
        }

        public EyeColor(int id):this()
        {
            Id = id;
        }

        public static Dictionary<int, string> EyeColors { get; set; }
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (EyeColors.ContainsKey(value))
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
