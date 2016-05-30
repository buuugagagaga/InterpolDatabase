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
        #endregion
        #region _private
        private static Dictionary<int, Сriminal> _criminals = new Dictionary<int, Сriminal>();
        private static Dictionary<int, CriminalGroup> _criminalGroups = new Dictionary<int, CriminalGroup>();
        #endregion
        #endregion
        #region Serialization
        public static void SaveData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("../../Storage/Data/criminals.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                binFormat.Serialize(stream, _criminals);
            using (Stream stream = new FileStream("../../Storage/Data/criminalGroups.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                binFormat.Serialize(stream, _criminalGroups);

        }
        public static void RestoreData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("../../Storage/Data/criminals.dat", FileMode.Open, FileAccess.Read, FileShare.None))
                _criminals = (Dictionary<int, Сriminal>)binFormat.Deserialize(stream);
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
            if(_criminals[id].CriminalGroupMembership!=null)    
                _criminals[id].UnsetCriminalGroup();
            _criminals.Remove(id);
        }

        #endregion
        #region _criminalGroups Actions
        public static void AddCriminalGroup(CriminalGroup criminalGroup) => _criminalGroups.Add(criminalGroup.Id, criminalGroup);
        public static void DeleteCriminalGroup(int id)
        {
            while (CriminalGroups[id].Members.Count > 0)
                CriminalGroups[id].Members.First().Value.UnsetCriminalGroup();
            _criminalGroups.Remove(id);
        }
        #endregion

        #region Other
        public static void ChangeCriminalsPhoto(string photoFilePath, int criminalId)
        {   
            Criminals[criminalId].PhotoFileName = MovePhotoToLibrary(photoFilePath, criminalId);
        }
        private static string MovePhotoToLibrary(string photoFilePath, int id)
        {
            if(!File.Exists(photoFilePath)) throw new FileNotFoundException();
            int count = 1;
            string fileName = id.ToString();
            string fileExtension = Path.GetExtension(photoFilePath);
            string path = "../../Storage/Files/";
            string newFullPath = path+fileName+fileExtension;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileName, count++);
                newFullPath = Path.Combine(path, tempFileName + fileExtension);
            }
            
            File.Copy(photoFilePath, newFullPath);
            return Path.GetFileName(newFullPath);
        }
        #endregion
    }
}