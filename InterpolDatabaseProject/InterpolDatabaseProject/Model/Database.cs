using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace InterpolDatabaseProject.Model
{
    [Serializable]
    public static class Database
    {
        #region private
        private static Dictionary<int, Сriminal> _criminals = new Dictionary<int, Сriminal> { {-1,null} };
        private static Dictionary<int, Crime> _crimes = new Dictionary<int, Crime> { { -1, null } };
        private static Dictionary<int, CriminalGroup> _criminalGroups = new Dictionary<int, CriminalGroup> { { -1, null } };
        private static Dictionary<int, List<int>> _criminalsToCrimes = new Dictionary<int, List<int>>();
        private static Dictionary<int, List<int>> _criminalsToCriminalGroups = new Dictionary<int, List<int>>();
        private static Dictionary<int, List<string>> _criminalsToPhotos = new Dictionary<int, List<string>>();
        #endregion
        #region public
        public static IReadOnlyDictionary<int, Сriminal> Criminals => _criminals;
        public static IReadOnlyDictionary<int, Crime> Crimes => _crimes;
        public static IReadOnlyDictionary<int, CriminalGroup> CriminalGroups => _criminalGroups;
        public static IReadOnlyDictionary<int, List<int>> CriminalToCrimes => _criminalsToCrimes;
        public static IReadOnlyDictionary<int, List<int>> CriminalsToCriminalGroups => _criminalsToCriminalGroups;
        public static IReadOnlyDictionary<int, List<string>> CriminalsToPhoto => _criminalsToPhotos;
        #endregion
        static Database()
        {

        }
        #region Serialization
        public static void SaveData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("Storage/Data/criminals.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(stream, _criminals);
            }
            using (Stream stream = new FileStream("Storage/Data/crimes.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(stream, _crimes);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalGroups.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(stream, _criminalGroups);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalsToCrimes.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(stream, _criminalsToCrimes);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalsToCriminalGroups.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(stream, _criminalsToCriminalGroups);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalsToPhotos.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(stream, _criminalsToPhotos);
            }
        }
        public static void RestoreData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("Storage/Data/criminals.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _criminals = (Dictionary<int, Сriminal>) binFormat.Deserialize(stream);
            }
            using (Stream stream = new FileStream("Storage/Data/crimes.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _crimes = (Dictionary<int, Crime>)binFormat.Deserialize(stream);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalGroups.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _criminalGroups = (Dictionary<int, CriminalGroup>)binFormat.Deserialize(stream);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalsToCrimes.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _criminalsToCrimes = (Dictionary<int, List<int>>)binFormat.Deserialize(stream);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalsToCriminalGroups.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _criminalsToCriminalGroups = (Dictionary<int, List<int>>)binFormat.Deserialize(stream);
            }
            using (Stream stream = new FileStream("Storage/Data/criminalsToPhotos.dat", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _criminalsToPhotos = (Dictionary<int, List<string>>)binFormat.Deserialize(stream);
            }
        }
        #endregion
        #region CRUD
        #region _criminals Methods
        public static void AddCriminal(Сriminal criminal)
        {
            _criminals.Add(_criminals.Keys.Last()+1, criminal);
        }
        public static void RemoveCriminal(int id)
        {
            _criminals.Remove(id);
            _criminalsToCrimes.Remove(id);
            _criminalsToCriminalGroups.Remove(id);
            _criminalsToPhotos.Remove(id);
        }
        public static void ChangeCriminal(int id, Сriminal criminal)
        {
            _criminals[id] = criminal;
        }
        #endregion
        #region _crimes Methods
        public static void AddCrime(Crime crime)
        {
            _crimes.Add(_crimes.Keys.Last()+1, crime);
        }
        public static void RemoveCrime(int id)
        {
            _crimes.Remove(id);
            for (int i = 0; i < _criminalsToCrimes.Count; i++)
            {
                if (_criminalsToCrimes[i].Contains(id))
                    _criminalsToCrimes[i].Remove(id);
            }
        }
        public static void ChangeCrime(int id, Crime crime)
        {
            _crimes[id] = crime;
        }
        #endregion
        #region _criminalGroups Methods
        public static void AddCriminalGroup(CriminalGroup criminalGroup)
        {
            _criminalGroups.Add(_criminalGroups.Keys.Last() + 1, criminalGroup);
        }
        public static void RemoveCriminalGroup(int id)
        {
            _criminalGroups.Remove(id);
            for (int i = 0; i < _criminalsToCriminalGroups.Count; i++)
            {
                if (_criminalsToCriminalGroups[i].Contains(id))
                    _criminalsToCriminalGroups[i].Remove(id);
            }
        }
        public static void ChangeCriminalGroup(int id, CriminalGroup criminalGroup)
        {
            _criminalGroups[id] = criminalGroup;
        }
        #endregion
        #endregion
        #region Tables Connections
        #region _criminalsToCrimes Connections
        public static void AddCriminalToCrimeConnection(int criminalId, int crimeId)
        {
            if(!_criminals.Keys.Contains(criminalId)) throw new ArgumentOutOfRangeException();
            if (!_crimes.Keys.Contains(crimeId)) throw new ArgumentOutOfRangeException();

            if(!_criminalsToCrimes.ContainsKey(criminalId))
                _criminalsToCrimes.Add(criminalId, new List<int>());
            if(!_criminalsToCrimes[criminalId].Contains(crimeId))
                _criminalsToCrimes[criminalId].Add(crimeId);
        }
        public static void RemoveCriminalToCrimeConnection(int criminalId, int crimeId)
        {
            if (!_criminals.Keys.Contains(criminalId)) throw new ArgumentOutOfRangeException();
            if (!_crimes.Keys.Contains(crimeId)) throw new ArgumentOutOfRangeException();

            if (_criminalsToCrimes.ContainsKey(criminalId))
                if(_criminalsToCrimes[criminalId].Contains(crimeId))
                    _criminalsToCrimes[criminalId].Remove(crimeId);
            if (_criminalsToCrimes[criminalId].Count == 0)
                _criminalsToCrimes.Remove(criminalId);
        }
        #endregion
        #region _criminalsToCriminalGroups Connections
        public static void AddCriminalToCriminalGroupConnection(int criminalId, int criminalGroupId)
        {
            if (!_criminals.Keys.Contains(criminalId)) throw new ArgumentOutOfRangeException();
            if (!_criminalGroups.Keys.Contains(criminalGroupId)) throw new ArgumentOutOfRangeException();

            if (!_criminalsToCriminalGroups.ContainsKey(criminalId))
                _criminalsToCriminalGroups.Add(criminalId, new List<int>());
            if (!_criminalsToCriminalGroups[criminalId].Contains(criminalGroupId))
                _criminalsToCriminalGroups[criminalId].Add(criminalGroupId);
        }
        public static void RemoveCriminalToCriminalGroupConnection(int criminalId, int criminalGroupId)
        {
            if (!_criminals.Keys.Contains(criminalId)) throw new ArgumentOutOfRangeException();
            if (!_criminalGroups.Keys.Contains(criminalGroupId)) throw new ArgumentOutOfRangeException();

            if (_criminalsToCriminalGroups.ContainsKey(criminalId))
                if(_criminalsToCriminalGroups[criminalId].Contains(criminalGroupId))
                    _criminalsToCriminalGroups[criminalId].Remove(criminalGroupId);
            if (_criminalsToCriminalGroups[criminalId].Count == 0)
                _criminalsToCrimes.Remove(criminalId);
        }
        #endregion
        #region _criminalsToPhotos
        public static void AddCriminalToPhotoConnection(int criminalId, string photoFileName)
        {
            if (!_criminals.Keys.Contains(criminalId)) throw new ArgumentOutOfRangeException();
            if (File.Exists("/Storage/Files/" + photoFileName)) throw new FileNotFoundException();

            if (!_criminalsToPhotos.ContainsKey(criminalId))
                _criminalsToPhotos.Add(criminalId, new List<string>());
            if (!_criminalsToPhotos[criminalId].Contains(photoFileName))
                _criminalsToPhotos[criminalId].Add(photoFileName);
        }
        public static void RemoveCriminalToPhotoConnection(int criminalId, string photoFileName)
        {
            if (!_criminals.Keys.Contains(criminalId)) throw new ArgumentOutOfRangeException();
            
            if (_criminalsToPhotos.ContainsKey(criminalId))
                if (_criminalsToPhotos[criminalId].Contains(photoFileName))
                    _criminalsToPhotos[criminalId].Remove(photoFileName);
            if (_criminalsToPhotos[criminalId].Count == 0)
                _criminalsToPhotos.Remove(criminalId);
        }
        #endregion
        #endregion
    }
}