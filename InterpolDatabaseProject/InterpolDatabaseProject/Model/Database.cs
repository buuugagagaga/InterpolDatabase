using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public static class Database
    {
        #region Properties
        #region _public
        public static ReadOnlyDictionary<int, Сriminal> Criminals => new ReadOnlyDictionary<int, Сriminal>(_criminals);
        public static ReadOnlyDictionary<int, CriminalGroup> CriminalGroups => new ReadOnlyDictionary<int, CriminalGroup>(_criminalGroups);
        public static ReadOnlyDictionary<int, Crime> Crimes => new ReadOnlyDictionary<int, Crime>(_crimes);
        #endregion
        #region _private
        private static Dictionary<int, Сriminal> _criminals = new Dictionary<int, Сriminal>();
        private static Dictionary<int, CriminalGroup> _criminalGroups = new Dictionary<int, CriminalGroup>();
        private static Dictionary<int, Crime> _crimes = new Dictionary<int, Crime>();
        #endregion
        #endregion
        static Database()
        {
            //RestoreData();
        }
        #region Serialization
        public static void SaveData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("../../Storage/Data/criminals.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                binFormat.Serialize(stream, _criminals);
            using (Stream stream = new FileStream("../../Storage/Data/crimes.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                binFormat.Serialize(stream, _crimes);
            using (Stream stream = new FileStream("../../Storage/Data/criminalGroups.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                binFormat.Serialize(stream, _criminalGroups);

        }
        public static void RestoreData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("../../Storage/Data/criminals.dat", FileMode.Open, FileAccess.Read, FileShare.None))
                _criminals = (Dictionary<int, Сriminal>)binFormat.Deserialize(stream);
            using (Stream stream = new FileStream("../../Storage/Data/crimes.dat", FileMode.Open, FileAccess.Read, FileShare.None))
                _crimes = (Dictionary<int, Crime>)binFormat.Deserialize(stream);
            using (Stream stream = new FileStream("../../Storage/Data/criminalGroups.dat", FileMode.Open, FileAccess.Read, FileShare.None))
                _criminalGroups = (Dictionary<int, CriminalGroup>)binFormat.Deserialize(stream);
        }
        #endregion


        #region _criminals Actions

        public static void AddCriminal(Сriminal criminal)
        {
            _criminals.Add(criminal.Id, criminal);
        }
        public static void DeleteCriminal(int id)
        {
            _criminals.Remove(id);
        }

        #endregion
        #region _crimes Actions
        public static void AddCrime(Crime crime) => _crimes.Add(crime.Id, crime); 
        public static void DeleteCrime(int id)
        {
            foreach (Сriminal сriminal in GetCrimeAссоmрliсеs(id))
                сriminal.DeleteCrime(id);
            _crimes.Remove(id);
        }
        public static List<Сriminal> GetCrimeAссоmрliсеs(int crimeId) => Criminals.Values.Where(criminal => criminal.Crimes.ContainsKey(crimeId)).ToList();
        #endregion
        #region _criminalGroups Actions
        public static void AddCriminalGroup(CriminalGroup criminalGroup) => _criminalGroups.Add(criminalGroup.Id, criminalGroup);
        public static void DeleteCriminalGroup(int id)
        {
            foreach (Сriminal criminalGroupMember in GetCriminalGroupMembers(id))
                criminalGroupMember.DeleteCriminalGroup(id);
            _criminalGroups.Remove(id);
        }
        public static List<Сriminal> GetCriminalGroupMembers(int criminalGroupId) => Criminals.Values.Where(criminal => criminal.CriminalGroups.ContainsKey(criminalGroupId)).ToList();
        #endregion

        #region Other

        public static void ChangeCriminalsPhoto(string photoFilePath, int criminalId)
        {   
            Criminals[criminalId].PhotoFileName = MovePhotoToLibrary(photoFilePath, criminalId);
        }

        private static string MovePhotoToLibrary(string photoFilePath, int id)
        {
            if(!File.Exists(photoFilePath)) throw new FileNotFoundException();
            string result = "" + id + photoFilePath.Substring(photoFilePath.LastIndexOf(".", StringComparison.Ordinal));
            File.Move(photoFilePath, "../../Storage/Data/"+result);
            return result;
        }

        #endregion
    }
}