using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public class Crime:ISerializable
    {
        private static int _lastId = -1;
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Country CommitmentCountry { get; set; }
        public string CommitmentPlace { get; set; }
        public string AdditionalData { get; set; }

        public Crime(string title, DateTime date, Country commitmentCountry, string commitmentPlace, string additionalData)
        {
            Id = ++_lastId;
            Title = title;
            Date = date;
            CommitmentCountry = commitmentCountry;
            CommitmentPlace = commitmentPlace;
            AdditionalData = additionalData;
        }
       
        public Crime(SerializationInfo info, StreamingContext context)
        {
            _lastId = info.GetInt32("static._lastId");
            Id = info.GetInt32("Id");
            Title = info.GetString("Title");
            Date = info.GetDateTime("Date");
            CommitmentCountry = (Country) info.GetValue("CommitmentCountry", typeof(Country));
            CommitmentPlace = info.GetString("CommitmentPlace");
            AdditionalData = info.GetString("AdditionalData");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("static._lastId", _lastId, typeof(int));
            info.AddValue("Id", Id, typeof(int));
            info.AddValue("Title", Title, typeof(string));
            info.AddValue("Date", Date, typeof(DateTime));
            info.AddValue("CommitmentCountry", CommitmentCountry, typeof(Country));
            info.AddValue("CommitmentPlace", CommitmentPlace, typeof(string));
            info.AddValue("AdditionalData", AdditionalData, typeof(string));
        }
    }
}