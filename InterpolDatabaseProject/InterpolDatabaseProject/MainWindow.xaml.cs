using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using InterpolDatabaseProject.Model;
using MahApps.Metro.Controls;

namespace InterpolDatabaseProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public class CriminalsListboxItemData
    {
        public int Id { get; set; }
        public string TotalName { get; }
        public string Citizenship { get; set; }
        public string Image { get; set; }
        public Brush TextColor { get; set; }
        public Сriminal.CriminalStateOptions State { get; set; }

        public CriminalsListboxItemData(int id, string forename, string codename, string lastname, int age, string citizenship, string imageName, Сriminal.CriminalStateOptions state)
        {
            Id = id;
            TotalName = forename + " " + codename + " " + lastname + ", AGE: " + age;
            Citizenship = citizenship;
            if (!File.Exists("..\\..\\Storage\\Files\\" + imageName))
                throw new FileNotFoundException();
            Image = "..\\..\\Storage\\Files\\" + imageName;

            State = state;
            switch (state)
            {
                case Сriminal.CriminalStateOptions.Busted:
                    TextColor = Brushes.Yellow;
                    break;
                case Сriminal.CriminalStateOptions.Wasted:
                    TextColor = Brushes.Red;
                    break;;
                case Сriminal.CriminalStateOptions.Wanted:
                    TextColor = Brushes.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }

    public partial class MainWindow
    {
        public List<Сriminal> FilteredСriminals => ApplyFiltersToCriminals(Database.Criminals);

        public List<CriminalsListboxItemData> CriminalsToShow
        {
            get
            {
                var result = FilteredСriminals.Select(criminal => new CriminalsListboxItemData(criminal.Id, criminal.Forename, criminal.CodeName, criminal.Lastname, criminal.Age, criminal.Citizenship.ToString(), (criminal.PhotosList.Count == 0) ? "default.png" : criminal.PhotosList[0], criminal.State)).ToList();
                return result;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //for (int i = 0; i < 250; i++)
            //{
            //    Database.AddCriminal(new Сriminal("lastname " + i, "forename " + i, "codename " + i, 100, new EyeColor(1), new HairColor(1),
            //        (Сriminal.SexOptions)(i % 3), new List<string>(), new Country(1), new Country(1), "place " + i,
            //        DateTime.Today, new Country(1), "place " + i, new List<Language> { new Language(1) },
            //        (Сriminal.CriminalStateOptions)(i % 3)));
            //    Database.AddCrime(new Crime("CrimeTitle " + i, DateTime.Today, new Country(0), "place " + i, "No data"));
            //    Database.AddCriminalGroup(new CriminalGroup("Group " + i, "No data"));

            //    Database.Criminals[i].AddCrime(Database.Crimes[i]);
            //    Database.Criminals[i].AddCriminalGroup(Database.CriminalGroups[i]);
            //}

            //Database.SaveData();
            Database.RestoreData();

            CriminalsListBox.ItemsSource = CriminalsToShow;
        }

        private List<Сriminal> ApplyFiltersToCriminals(ReadOnlyDictionary<int, Сriminal> criminals)
        {
            List<Сriminal> result = new List<Сriminal>(criminals.Values);
            ///todo: Написать фильтрацию
            return result;
        }

        public static class Filter
        {

        }
    }
}
