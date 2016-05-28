using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
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
                var result = FilteredСriminals.Select(criminal => new CriminalsListboxItemData(criminal.Id, criminal.Forename, criminal.CodeName, criminal.Lastname, criminal.Age, criminal.Citizenship.ToString(), criminal.PhotoFileName ?? "default.png", criminal.State)).ToList();
                return result;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 250; i++)
            {
                Database.AddCriminal(new Сriminal("lastname " + i, "forename " + i, "codename " + i, 100, new EyeColor(1), new HairColor(1),
                    (Сriminal.SexOptions)(i % 3), "no data", new Country(1), new Country(1), "place " + i,
                    DateTime.Today, new Country(1), "place " + i, new List<Language> { new Language(1) },
                    (Сriminal.CriminalStateOptions)(i % 3), null));
                Database.AddCrime(new Crime("CrimeTitle " + i, DateTime.Today, new Country(0), "place " + i, "No data"));
                Database.AddCriminalGroup(new CriminalGroup("Group " + i, "No data"));

                Database.Criminals[i].AddCrime(Database.Crimes[i]);
                Database.Criminals[i].AddCriminalGroup(Database.CriminalGroups[i]);
            }

            Database.SaveData();
            Database.RestoreData();

            InitializeComponent();
            SetDataSources();
            NewCriminalFlyout.IsOpen = true;
        }

        private List<Сriminal> ApplyFiltersToCriminals(ReadOnlyDictionary<int, Сriminal> criminals)
        {
            List<Сriminal> result = new List<Сriminal>(criminals.Values);
            ///todo: Написать фильтрацию
            return result;
        }

        private void SetDataSources()
        {
            CriminalsListBox.ItemsSource = CriminalsToShow;
            AddNewCriminal_EyeColorComboBox.ItemsSource = EyeColor.EyeColors;
            AddNewCriminal_HairColorComboBox.ItemsSource = HairColor.HairColors;
            AddNewCriminal_SexComboBox.ItemsSource = Enum.GetValues(typeof(Сriminal.SexOptions)).Cast<Сriminal.SexOptions>();
            AddNewCriminal_CitizenshipComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_BirthCountryComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_BirthdateDatePicker.SelectedDate = default(DateTime);
            AddNewCriminal_LastLivingCountryComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_CurrentStateComboBox.ItemsSource = Enum.GetValues(typeof(Сriminal.CriminalStateOptions)).Cast<Сriminal.CriminalStateOptions>();
            AddNewCriminal_LanguagesListBox.ItemsSource = Model.Language.Languages;
        }



        public static class Filter
        {

        }

        private void AddNewCriminal_PickButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter =
                    "Images (*.jpeg, *.png, *.jpg)|*.jpeg;*.png;*.jpg"
            };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                AddNewCriminal_PhotoFilePath.Text = dlg.FileName;
            }
        }
        private void AddNewCriminal_SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //List<Language> 
            //Database.AddCriminal(new Сriminal(AddNewCriminal_LastnameTextBox.Text,
            //    AddNewCriminal_ForenameTextBox.Text,
            //    AddNewCriminal_CodenameTextBox.Text,
            //    AddNewCriminal_HeightSlider.Value,
            //    new EyeColor(AddNewCriminal_EyeColorComboBox.SelectedIndex),
            //    new HairColor(AddNewCriminal_HairColorComboBox.SelectedIndex),
            //    (Сriminal.SexOptions)AddNewCriminal_SexComboBox.SelectedIndex,
            //    AddNewCriminal_SpecialSignsTextBox.Text,
            //    new Country(AddNewCriminal_CitizenshipComboBox.SelectedIndex),
            //    new Country(AddNewCriminal_BirthCountryComboBox.SelectedIndex),
            //    AddNewCriminal_BirthplaceTextBox.Text,
            //    AddNewCriminal_BirthdateDatePicker.SelectedDate,
            //    new Country(AddNewCriminal_LastLivingCountryComboBox.SelectedIndex),
            //    AddNewCriminal_LastLivingPlaceTextBox.Text,
                    
            //    ));
        }
    }
}
