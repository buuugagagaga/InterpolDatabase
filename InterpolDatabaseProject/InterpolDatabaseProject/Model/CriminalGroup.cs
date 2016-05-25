using System;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public class CriminalGroup
    {
        public string Name { get; set; }
        public string AdditionalData { get; set; }

        public CriminalGroup()
        {
            Name = "Unknown";
            AdditionalData = "No data";
        }

        public CriminalGroup(string name, string additionalData)
        {
            Name = name;
            AdditionalData = additionalData;
        }
    }
}
