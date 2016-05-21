using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolDatabaseProject.Model
{
    public class CriminalGroup
    {
        public int Id { set; get; }
        public string Name { get; set; }
        public string AdditionalData { get; set; }

        public CriminalGroup()
        {
            Id = -1;
            Name = "Unknown";
            AdditionalData = "No data";
        }

        public CriminalGroup(int id, string name, string additionalData)
        {
            Id = id;
            Name = name;
            AdditionalData = additionalData;
        }
    }
}
