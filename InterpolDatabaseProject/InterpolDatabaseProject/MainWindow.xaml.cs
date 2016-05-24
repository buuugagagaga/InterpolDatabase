using System;
using System.Collections.Generic;
using System.Windows;
using InterpolDatabaseProject.Model;

namespace InterpolDatabaseProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Database.RestoreData();

            //for (int i = 1; i < 250; i++)
            //{
            //    Сriminal criminal = new Сriminal("Lastname" + i, "Forename" + i, "Codename" + i, 100, new EyeColor(0), new HairColor(0), Сriminal.SexOptions.Unknown, new List<string> { "Something" }, new Country(0), new Country(0), "Unknown", DateTime.MaxValue, new Country(0), "Unknown", new List<Language> { new Language(0) });
            //    Crime crime = new Crime(new Crime.CrimeType(0), DateTime.MaxValue, new Country(0), "Unknown" + i, "No data" + i);
            //    CriminalGroup criminalGroup = new CriminalGroup("Cosa nostra" + i, "No data" + i);


            //    Database.AddCrime(crime);
            //    Database.AddCriminal(criminal);
            //    Database.AddCriminalGroup(criminalGroup);
            //    Database.AddCriminalToCrimeConnection(i, i);
            //    Database.AddCriminalToCrimeConnection(i, i-1);
            //    Database.AddCriminalToCriminalGroupConnection(i, i);
            //    Database.AddCriminalToCriminalGroupConnection(i, i-1);
            //    Database.AddCriminalToPhotoConnection(0, "123123.txt");
            //}
            
            //Database.SaveData();
            
            Country country = new Country(2);
            EyeColor eyeColor = new EyeColor(1);
            HairColor hairColor = new HairColor(3);
            Country country2 = new Country(2);
        }
    }
}
