using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public class CriminalGroup : ISerializable, IEquatable<CriminalGroup>
    {
        #region Fields
        /// <summary>
        /// Отображает последний использованный для группировки ID 
        /// </summary>
        private static int _lastId = -1;
        #endregion
        #region Constructors
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="name">Название группировки</param>
        /// <param name="additionalData">Дополнительная информация</param>
        public CriminalGroup(string name, string additionalData)
        {
            Id = ++_lastId;
            Name = name;
            AdditionalData = additionalData;
        }

        /// <summary>
        /// Конструктор для десериализации
        /// </summary>
        public CriminalGroup(SerializationInfo info, StreamingContext context)
        {
            _lastId = info.GetInt32("static._lastId");
            Id = info.GetInt32("Id");
            Name = info.GetString("Name");
            AdditionalData = info.GetString("AdditionalData");
        }
        #endregion
        #region Properties
        /// <summary>
        /// Уникальный номер группировки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название группировки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дополнительная информация о группировке
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Коллекция членов группировки. Только для чтения
        /// </summary>
        public ReadOnlyDictionary<int, Сriminal> Members 
            => new ReadOnlyDictionary<int, Сriminal>(_members);
        private readonly Dictionary<int, Сriminal> _members 
            = new Dictionary<int, Сriminal>();
        #endregion
        #region Methods
        /// <summary>
        /// Добавление члена в группировку
        /// Контролирует корректность поля CriminalGroupMembership аргумента criminal
        /// </summary>
        /// <param name="criminal">Преступник, добавляемый в группировку</param>
        public void AddMember(Сriminal criminal)
        {
            if (_members.ContainsKey(criminal.Id))
                _members.Remove(criminal.Id);
            _members.Add(criminal.Id, criminal);
            criminal.SetCriminalGroup(this);
        }

        /// <summary>
        /// Удаление преступника из группировки
        /// Контролирует корректность поля CriminalGroupMembership аргумента criminal
        /// </summary>
        /// <param name="id">Уникальный номер преступника</param>
        public void RemoveMember(int id)
        {
            if(!_members.ContainsKey(id)) throw new KeyNotFoundException();
            Сriminal criminal = _members[id];
            _members.Remove(id);
            criminal.UnsetCriminalGroup();
        }

        /// <summary>
        /// Заполняет SerializationInfo данными, необходимыми для сериализации
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("static._lastId", _lastId, typeof(int));
            info.AddValue("Id", Id, typeof(int));
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("AdditionalData", AdditionalData, typeof(string));
        }

        /// <summary>
        /// Реализация интерфейса IEquatable<CriminalGroup>
        /// </summary>
        public bool Equals(CriminalGroup other)
        {
            return Id == other.Id;
        }

        /// <summary>
        /// Преобразование данных группировки в строку
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
