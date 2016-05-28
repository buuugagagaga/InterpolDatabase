using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Dictionary<int, Сriminal> _members = new Dictionary<int, Сriminal>();
        public ReadOnlyDictionary<int, Сriminal> Members => new ReadOnlyDictionary<int, Сriminal>(_members);

        public void AddMember(Сriminal criminal)
        {
            if (_members.ContainsKey(criminal.Id))
                _members.Remove(criminal.Id);
            _members.Add(criminal.Id, criminal);
            criminal.SetCriminalGroup(this);
        }

        public void RemoveMember(int id)
        {
            if(!_members.ContainsKey(id)) throw new KeyNotFoundException();
            Сriminal criminal = _members[id];
            _members.Remove(id);
            criminal.UnsetCriminalGroup();
        }

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

        public override string ToString()
        {
            return Name;
        }
    }
}
