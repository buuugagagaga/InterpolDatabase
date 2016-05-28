using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        public string Citizenship { get; set; }
        public BitmapImage Image { get; set; }
        public Brush TextColor { get; set; }
        public Сriminal.CriminalStateOptions State { get; set; }

        public CriminalsListboxItemData(int id, string forename, string codename, string lastname, int age, string citizenship, string imageName, Сriminal.CriminalStateOptions state)
        {
            Id = id;
            TotalName = forename + " " + codename + " " + lastname + ", AGE: " + age;
            Citizenship = citizenship;
            if (!File.Exists("..\\..\\Storage\\Files\\" + imageName))
                throw new FileNotFoundException();
            Image = new BitmapImage(new Uri(Path.GetFullPath("..\\..\\Storage\\Files\\" + imageName)));

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
            //for (int i = 0; i < 250; i++)
            //{
            //    Database.AddCriminal(new Сriminal("lastname " + i, "forename " + i, "codename " + i, 100, new EyeColor(1), new HairColor(1),
            //        (Сriminal.SexOptions)(i % 3), "no data", new Country(1), new Country(1), "place " + i,
            //        DateTime.Today, new Country(1), "place " + i, new List<Language> { new Language(1) },
            //        (Сriminal.CriminalStateOptions)(i % 3), null,null, new List<Crime>()));
            //    Database.AddCriminalGroup(new CriminalGroup("Group " + i, "No data"));
            //    Database.Criminals[i].SetCriminalGroup(Database.CriminalGroups[i]);
            //}

            Database.RestoreData();

            InitializeComponent();
            SetDataSources();
            ReloadObservableData();
            AddNewCriminal.IsOpen = true;
        }

        private List<Сriminal> ApplyFiltersToCriminals(ReadOnlyDictionary<int, Сriminal> criminals)
        {
            List<Сriminal> result = new List<Сriminal>(criminals.Values);

            result = result.Where(r => r.Forename.ToLower().Contains(Filter_ForenameTextBox.Text.ToLower())).ToList();
            result = result.Where(r => r.CodeName.ToLower().Contains(Filter_CodenameTextBox.Text.ToLower())).ToList();
            result = result.Where(r => r.Lastname.ToLower().Contains(Filter_LastnameTextBox.Text.ToLower())).ToList();

            ///todo: Написать фильтрацию
            return result;
        }

        private void SetDataSources()
        {
            AddNewCriminal_EyeColorComboBox.ItemsSource = EyeColor.EyeColors;
            AddNewCriminal_HairColorComboBox.ItemsSource = HairColor.HairColors;
            AddNewCriminal_SexComboBox.ItemsSource = Enum.GetValues(typeof(Сriminal.SexOptions)).Cast<Сriminal.SexOptions>();
            AddNewCriminal_CitizenshipComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_BirthCountryComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_LastLivingCountryComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_CurrentStateComboBox.ItemsSource = Enum.GetValues(typeof(Сriminal.CriminalStateOptions)).Cast<Сriminal.CriminalStateOptions>();
            AddNewCriminal_LanguagesListBox.ItemsSource = Model.Language.Languages;
            AddNewCriminal_CriminalGroupComboBox.ItemsSource = Database.CriminalGroups;
            AddNewCriminal_ChargesListBox.ItemsSource = Crime.Crimes;

        }
        private void ReloadObservableData()
        {
            CriminalsListBox.ItemsSource = CriminalsToShow;
            AddNewCriminal_CriminalGroupComboBox.ItemsSource = Database.CriminalGroups;
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
            List<Language> languages = (from string selectedItem in AddNewCriminal_LanguagesListBox.SelectedItems
                                        select new Language(Model.Language.Languages.IndexOf(selectedItem))).ToList();
            List<Crime> charges = (from string selectedItem in AddNewCriminal_ChargesListBox.SelectedItems
                                   select new Crime(Model.Crime.Crimes.IndexOf(selectedItem))).ToList();

            Database.AddCriminal(new Сriminal(
                AddNewCriminal_LastnameTextBox.Text,
                AddNewCriminal_ForenameTextBox.Text,
                AddNewCriminal_CodenameTextBox.Text,
                Convert.ToInt32(AddNewCriminal_HeightSlider.Value),
                new EyeColor(AddNewCriminal_EyeColorComboBox.SelectedIndex),
                new HairColor(AddNewCriminal_HairColorComboBox.SelectedIndex),
                (Сriminal.SexOptions) AddNewCriminal_SexComboBox.SelectedIndex,
                AddNewCriminal_SpecialSignsTextBox.Text,
                new Country(AddNewCriminal_CitizenshipComboBox.SelectedIndex),
                new Country(AddNewCriminal_BirthCountryComboBox.SelectedIndex),
                AddNewCriminal_BirthplaceTextBox.Text,
                AddNewCriminal_BirthdateDatePicker.SelectedDate,
                new Country(AddNewCriminal_LastLivingCountryComboBox.SelectedIndex),
                AddNewCriminal_LastLivingPlaceTextBox.Text,
                languages,
                (Сriminal.CriminalStateOptions) AddNewCriminal_CurrentStateComboBox.SelectedIndex,
                null,
                (AddNewCriminal_CriminalGroupComboBox.SelectedIndex == -1)
                    ? null
                    : Database.CriminalGroups[AddNewCriminal_CriminalGroupComboBox.SelectedIndex],
                charges
                )
                );

            if(AddNewCriminal_PhotoFilePath.Text!="")
                Database.ChangeCriminalsPhoto(AddNewCriminal_PhotoFilePath.Text, Database.Criminals.Count-1);

            ReloadObservableData();
            AddNewCriminal.IsOpen = false;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Database.SaveData();
        }

        private void CriminalActions_AddCriminalButton_OnClick(object sender, RoutedEventArgs e) => AddNewCriminal.IsOpen = true;
        
        private void Flyout_OnIsOpenChanged(object sender, RoutedEventArgs e)=>MainGrid.IsEnabled = !MainGrid.IsEnabled;

        private void CriminalActions_DeleteCriminalButton_OnClick(object sender, RoutedEventArgs e)
        {
            Database.DeleteCriminal(((CriminalsListboxItemData)CriminalsListBox.SelectedItem).Id);
            ReloadObservableData();
        }

        private void CriminalsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => CriminalActions_DeleteCriminalButton.IsEnabled = CriminalsListBox.SelectedItems.Count!=0;

        private void Filter_LastnameTextBox_OnTextChanged(object sender, TextChangedEventArgs e) => ReloadObservableData();

        private void Filter_ForenameTextBox_OnTextChanged(object sender, TextChangedEventArgs e) => ReloadObservableData();

        private void Filter_CodenameTextBox_OnTextChanged(object sender, TextChangedEventArgs e) => ReloadObservableData();
    }
}
