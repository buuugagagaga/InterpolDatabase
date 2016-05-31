using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace InterpolDatabaseProject.Model
{
    /// <summary>
    /// Класс для управления данными программы
    /// </summary>
    [Serializable]
    public static class Database
    {
        #region Properties
        #region _public
        /// <summary>
        /// Коллекция преступников. Только для чтения.
        /// </summary>
        public static ReadOnlyDictionary<int, Сriminal> Criminals => new ReadOnlyDictionary<int, Сriminal>(_criminals);

        /// <summary>
        /// Коллекция группировок. Только для чтения
        /// </summary>
        public static ReadOnlyDictionary<int, CriminalGroup> CriminalGroups => new ReadOnlyDictionary<int, CriminalGroup>(_criminalGroups);
        #endregion
        #region _private
        private static Dictionary<int, Сriminal> _criminals = new Dictionary<int, Сriminal>();
        private static Dictionary<int, CriminalGroup> _criminalGroups = new Dictionary<int, CriminalGroup>();
        #endregion
        #endregion
        #region Methods
        #region Serialization
        /// <summary>
        /// Метод для сериализации данных 
        /// </summary>
        public static void SaveData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("../../Storage/Data/criminals.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                binFormat.Serialize(stream, _criminals);
            using (Stream stream = new FileStream("../../Storage/Data/criminalGroups.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                binFormat.Serialize(stream, _criminalGroups);

        }
        /// <summary>
        /// Метод для восстановления данных данных.
        /// </summary>
        public static void RestoreData()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream stream = new FileStream("../../Storage/Data/criminalGroups.dat", FileMode.Open, FileAccess.Read, FileShare.None))
                _criminalGroups = (Dictionary<int, CriminalGroup>)binFormat.Deserialize(stream);
            using (Stream stream = new FileStream("../../Storage/Data/criminals.dat", FileMode.Open, FileAccess.Read, FileShare.None))
                _criminals = (Dictionary<int, Сriminal>)binFormat.Deserialize(stream);

            //Cвязывание преступных группировок с преступниками
            foreach (var criminal in Criminals)
            {
                if(criminal.Value.CriminalGroupMembership!=null)
                    criminal.Value.SetCriminalGroup(CriminalGroups[criminal.Value.CriminalGroupMembership.Id]);
            }
        }
        #endregion
        #region _criminals Actions
        /// <summary>
        /// Добавление преступника в коллекцию.
        /// Контролирует совпадение ключа в словаре с ID преступника.
        /// </summary>
        /// <param name="criminal">Преступник, которого добавляем</param>
        public static void AddCriminal(Сriminal criminal)
        {
            _criminals.Add(criminal.Id, criminal);
        }
        
        /// <summary>
        /// Удаление преступника из коллекции
        /// Контролирует также удаление преступника из списка членов группировки
        /// </summary>
        /// <param name="id">Уникальный номер преступника</param>
        public static void DeleteCriminal(int id)
        {
            _criminals[id].UnsetCriminalGroup();
            _criminals.Remove(id);
        }

        #endregion
        #region _criminalGroups Actions
        /// <summary>
        /// Добавление группировки в коллекцию
        /// </summary>
        /// <param name="criminalGroup">Добавляемая группировка</param>
        public static void AddCriminalGroup(CriminalGroup criminalGroup) => _criminalGroups.Add(criminalGroup.Id, criminalGroup);
        
        /// <summary>
        /// Метод для удаления группировки из коллекции
        /// </summary>
        /// <param name="id">Уникальный номер группировки</param>
        public static void DeleteCriminalGroup(int id)
        {
            while (CriminalGroups[id].Members.Count > 0)
                CriminalGroups[id].Members.First().Value.UnsetCriminalGroup();
            _criminalGroups.Remove(id);
        }
        #endregion
        #region Other
        /// <summary>
        /// Метод для изменения фотографии преступника
        /// </summary>
        /// <param name="photoFilePath">Путь к фотографии</param>
        /// <param name="criminalId">Уникальный номер преступника</param>
        public static void ChangeCriminalsPhoto(string photoFilePath, int criminalId)
        {
            Criminals[criminalId].PhotoFileName = MovePhotoToLibrary(photoFilePath, criminalId);
        }

        /// <summary>
        /// Метод для сохраниения фотографии в каталоге программы
        /// </summary>
        /// <param name="photoFilePath">Путь к фотографии</param>
        /// <param name="id">Уникальный номер преступника</param>
        /// <returns>Конечное имя файла</returns>
        private static string MovePhotoToLibrary(string photoFilePath, int id)
        {
            if (!File.Exists(photoFilePath)) throw new FileNotFoundException();
            int count = 1;
            string fileName = id.ToString();
            string fileExtension = Path.GetExtension(photoFilePath);
            string path = "../../Storage/Files/";
            string newFullPath = path + fileName + fileExtension;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileName, count++);
                newFullPath = Path.Combine(path, tempFileName + fileExtension);
            }

            File.Copy(photoFilePath, newFullPath);
            return Path.GetFileName(newFullPath);
        }
        #endregion
        #endregion
    }
}