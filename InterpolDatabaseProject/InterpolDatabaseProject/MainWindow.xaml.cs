using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using InterpolDatabaseProject.Model;

namespace InterpolDatabaseProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public class CriminalsListboxItemData
    {
        public int Id { get; set; }
        public string TotalName { get; }
        public string Image { get; set; }
        public Brush TransparentBrush { get; set; }
        public Сriminal.CriminalStateOptions State { get; set; }

        public CriminalsListboxItemData(int id, string forename, string codename, string lastname, string imageName, Сriminal.CriminalStateOptions state)
        {
            Id = id;
            TotalName = forename + " " + codename + " " + lastname;
            if(!File.Exists("..\\..\\Storage\\Files\\" + imageName))
                throw new FileNotFoundException();
            Image = "..\\..\\Storage\\Files\\" + imageName;

            State = state;
            if (State == Сriminal.CriminalStateOptions.Wanted)
                TransparentBrush = Brushes.Yellow;
            if (State == Сriminal.CriminalStateOptions.Busted)
                TransparentBrush = Brushes.DarkRed;
            if (State == Сriminal.CriminalStateOptions.Wasted)
                TransparentBrush = Brushes.Gray;
            
        }


    }

    public partial class MainWindow : Window
    {
        public List<CriminalsListboxItemData> CriminalsToShow
        {
            get
            {
                var result = Database.Criminals.Select(criminal => new CriminalsListboxItemData(
                   criminal.Key,
                   criminal.Value.Forename,
                   criminal.Value.CodeName,
                   criminal.Value.Lastname,
                   "default.jpg",
                   criminal.Value.State)
                   ).ToList();
                return result;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            CriminalsListBox.ItemsSource = CriminalsToShow;

            //for (int i = 0; i < 250; i++)
            //{
            //    Сriminal criminal = new Сriminal("Lastname" + i, "Forename" + i, "Codename" + i, 100, new EyeColor(0), new HairColor(0), Сriminal.SexOptions.Unknown, new List<string> { "Something" }, new Country(0), new Country(0), "Unknown", DateTime.MaxValue, new Country(0), "Unknown", new List<Language> { new Language(0) });
            //    Crime crime = new Crime(new Crime.CrimeType(0), DateTime.MaxValue, new Country(0), "Unknown" + i, "No data" + i);
            //    CriminalGroup criminalGroup = new CriminalGroup("Cosa nostra" + i, "No data" + i);

            //    Database.AddCrime(crime);
            //    Database.AddCriminal(criminal);
            //    Database.AddCriminalGroup(criminalGroup);
            //    Database.AddCriminalToCrimeConnection(i, i);
            //    Database.AddCriminalToCriminalGroupConnection(i, i);
            //}

            Database.SaveData();


        }
    }
}
