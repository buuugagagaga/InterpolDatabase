using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public class CriminalGroup : ISerializable
    {
        private static int _lastId = -1;

        public int Id { get; set; }
        public string Name { get; set; }
        public string AdditionalData { get; set; }

        public CriminalGroup(string name, string additionalData)
        {
            Id = ++_lastId;
            Name = name;
            AdditionalData = additionalData;
        }

        public CriminalGroup(SerializationInfo info, StreamingContext context)
        {
            _lastId = info.GetInt32("static._lastId");
            Id = info.GetInt32("Id");
            Name = info.GetString("Name");
            AdditionalData = info.GetString("AdditionalData");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("static._lastId", _lastId, typeof(int));
            info.AddValue("Id", Id, typeof(int));
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("AdditionalData", AdditionalData, typeof(string));
        }
    }
}
