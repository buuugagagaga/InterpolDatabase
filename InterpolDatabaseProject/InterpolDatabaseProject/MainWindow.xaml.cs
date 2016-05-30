using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls.Dialogs;
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

        public CriminalsListboxItemData(int id, string forename, string codename, string lastname, int? age, string citizenship, string imageName, Сriminal.CriminalStateOptions state)
        {
            Id = id;
            TotalName = "#"+ id + " - " + forename + " \"" + codename + "\" " + lastname + " AGE: " + ((age == null) ? "-" : age.ToString());
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
                    break; ;
                case Сriminal.CriminalStateOptions.Wanted:
                    TextColor = Brushes.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }

    public static class Filter
    {
        public static List<Сriminal> ByState(List<Сriminal> result, IList selectedItems)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (selectedItems.Cast<Сriminal.CriminalStateOptions>().Any(selectedItem => result[i].State == selectedItem)) continue;
                result.RemoveAt(i);
                i--;
            }
            return result;
        }
        public static List<Сriminal> ByForename(List<Сriminal> result, string text) => result.Where(r => r.Forename.ToLower().Contains(text.ToLower())).ToList();
        public static List<Сriminal> ByCodename(List<Сriminal> result, string text) => result.Where(r => r.CodeName.ToLower().Contains(text.ToLower())).ToList();
        public static List<Сriminal> ByLastname(List<Сriminal> result, string text) => result.Where(r => r.Lastname.ToLower().Contains(text.ToLower())).ToList();
        public static List<Сriminal> ByAge(List<Сriminal> result, int lowerValue, int upperValue) => result.Where(r => r.Age >= lowerValue && r.Age <= upperValue).ToList();
        public static List<Сriminal> ByAge(List<Сriminal> result) => result.Where(r => r.Age == null).ToList();
        public static List<Сriminal> ByHeight(List<Сriminal> result, int lowerValue, int upperValue) => result.Where(r => r.Height >= lowerValue && r.Height <= upperValue).ToList();
        public static List<Сriminal> ByHeight(List<Сriminal> result) => result.Where(r => r.Height == null).ToList();
        public static List<Сriminal> ByEyeColor(List<Сriminal> result, int id) => result.Where(r => r.ColorOfEye.Id == id).ToList();
        public static List<Сriminal> ByHairColor(List<Сriminal> result, int id) => result.Where(r => r.ColorOfHair.Id == id).ToList();
        public static List<Сriminal> BySex(List<Сriminal> result, string selectedValue) => result.Where(r => r.Sex.ToString() == selectedValue).ToList();
        public static List<Сriminal> BySpecialSigns(List<Сriminal> result, string text) => result.Where(r => r.SpecialSigns.ToLower().Contains(text.ToLower())).ToList();
        public static List<Сriminal> ByLanguages(List<Сriminal> result, IList selectedItems)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (selectedItems.Cast<string>().All(selectedItem => result[i].Languages.Contains(new Language(Model.Language.Languages.IndexOf(selectedItem))))) continue;
                result.RemoveAt(i);
                i--;
            }
            return result;
        }
        public static List<Сriminal> ByCitizenship(List<Сriminal> result, int id) => result.Where(r=>r.Citizenship.Id == id).ToList();
        public static List<Сriminal> ByBirthCountry(List<Сriminal> result, int id) => result.Where(r => r.BirthCountry.Id == id).ToList();
        public static List<Сriminal> ByBirthPlace(List<Сriminal> result, string text) => result.Where(r => r.Birthplace.ToLower().Contains(text.ToLower())).ToList();
        public static List<Сriminal> ByLastLivingCountry(List<Сriminal> result, int id) => result.Where(r => r.LastLivingCountry.Id == id).ToList();
        public static List<Сriminal> ByLastLivingPlace(List<Сriminal> result, string text) => result.Where(r => r.LastLivingPlace.ToLower().Contains(text.ToLower())).ToList();
    }

    public partial class MainWindow 
    {
        public List<Сriminal> FilteredСriminals => ApplyFiltersToCriminals(Database.Criminals);

        public List<CriminalsListboxItemData> CriminalsToShow
        {
            get
            {
                var result =
                    FilteredСriminals.Select(
                        criminal =>
                            new CriminalsListboxItemData(criminal.Id, criminal.Forename, criminal.CodeName,
                                criminal.Lastname, criminal.Age, criminal.Citizenship.ToString(),
                                criminal.PhotoFileName ?? "default.png", criminal.State)).ToList();
                return result;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Database.RestoreData();

            InitializeComponent();
            SetDataSources();
            Reload_CriminalsListBox();
            Reload_CriminalGroups();
        }

        private List<Сriminal> ApplyFiltersToCriminals(ReadOnlyDictionary<int, Сriminal> criminals)
        {
            var result = CriminalGroupsListBox.SelectedItems.Count == 0 ? new List<Сriminal>(criminals.Values) : new List<Сriminal>(((KeyValuePair<int, CriminalGroup>) CriminalGroupsListBox.SelectedItem).Value.Members.Values);

            if (Filter_StateListBox.SelectedItems.Count > 0) result = Filter.ByState(result, Filter_StateListBox.SelectedItems);

            if (Filter_ForenameTextBox.Text != "") result = Filter.ByForename(result, Filter_ForenameTextBox.Text);
            if (Filter_CodenameTextBox.Text != "") result = Filter.ByCodename(result, Filter_CodenameTextBox.Text);
            if (Filter_LastnameTextBox.Text != "") result = Filter.ByLastname(result, Filter_LastnameTextBox.Text);
            
            if (!Filter_IsAgeKnown.IsChecked.Value)
                result = Filter.ByAge(result);
            else if (Filter_AgeRangeSlider.LowerValue > Filter_AgeRangeSlider.Minimum || Filter_AgeRangeSlider.UpperValue < Filter_AgeRangeSlider.Maximum)
                result = Filter.ByAge(result, (int)Filter_AgeRangeSlider.LowerValue, (int)Filter_AgeRangeSlider.UpperValue);

            if (!Filter_IsHeightKnown.IsChecked.Value)
                result = Filter.ByHeight(result);
            else if (Filter_HeightRangeSlider.LowerValue > Filter_HeightRangeSlider.Minimum || Filter_HeightRangeSlider.UpperValue < Filter_HeightRangeSlider.Maximum)
                result = Filter.ByHeight(result, (int)Filter_HeightRangeSlider.LowerValue, (int)Filter_HeightRangeSlider.UpperValue);

            if (Filter_EyeColorCombobox.SelectedIndex != -1) result = Filter.ByEyeColor(result, Filter_EyeColorCombobox.SelectedIndex);
            if (Filter_HairColorCombobox.SelectedIndex != -1) result = Filter.ByHairColor(result, Filter_HairColorCombobox.SelectedIndex);
            if (Filter_SexCombobox.SelectedIndex != -1) result = Filter.BySex(result, Filter_SexCombobox.SelectedValue.ToString());
            if (Filter_SpecialSignsTextbox.Text != "") result = Filter.BySpecialSigns(result, Filter_SpecialSignsTextbox.Text);
            if (Filter_LanguagesListbox.SelectedItems.Count > 0) result = Filter.ByLanguages(result, Filter_LanguagesListbox.SelectedItems);
            if (Filter_CitizenshipComboBox.SelectedIndex != -1) result = Filter.ByCitizenship(result, Filter_CitizenshipComboBox.SelectedIndex);
            if (Filter_BirthCountryComboBox.SelectedIndex != -1) result = Filter.ByBirthCountry(result, Filter_BirthCountryComboBox.SelectedIndex);
            if(Filter_PlaceOfBirthTextBox.Text != "") result = Filter.ByBirthPlace(result, Filter_PlaceOfBirthTextBox.Text);
            if (Filter_LastLivingCountryComboBox.SelectedIndex != -1) result = Filter.ByLastLivingCountry(result, Filter_LastLivingCountryComboBox.SelectedIndex);
            if (Filter_LastLivingPlaceTextBox.Text != "") result = Filter.ByLastLivingPlace(result, Filter_LastLivingPlaceTextBox.Text);
            
            return result;
        }

        private void SetDataSources()
        {
            AddNewCriminal_EyeColorComboBox.ItemsSource = EyeColor.EyeColors;
            AddNewCriminal_HairColorComboBox.ItemsSource = HairColor.HairColors;
            AddNewCriminal_SexComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.SexOptions)).Cast<Сriminal.SexOptions>();
            AddNewCriminal_CitizenshipComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_BirthCountryComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_LastLivingCountryComboBox.ItemsSource = Country.Countries;
            AddNewCriminal_CurrentStateComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.CriminalStateOptions)).Cast<Сriminal.CriminalStateOptions>();
            AddNewCriminal_LanguagesListBox.ItemsSource = Model.Language.Languages;
            AddNewCriminal_ChargesListBox.ItemsSource = Crime.Crimes;

            Filter_EyeColorCombobox.ItemsSource = EyeColor.EyeColors;
            Filter_HairColorCombobox.ItemsSource = HairColor.HairColors;
            Filter_SexCombobox.ItemsSource = Enum.GetValues(typeof(Сriminal.SexOptions)).Cast<Сriminal.SexOptions>();
            Filter_LanguagesListbox.ItemsSource = Model.Language.Languages;
            Filter_CitizenshipComboBox.ItemsSource = Country.Countries;
            Filter_BirthCountryComboBox.ItemsSource = Country.Countries;
            Filter_LastLivingCountryComboBox.ItemsSource = Country.Countries;
            Filter_StateListBox.ItemsSource = Enum.GetValues(typeof(Сriminal.CriminalStateOptions)).Cast<Сriminal.CriminalStateOptions>();
        }

        private void Reload_CriminalsListBox()
        {
            if (CriminalsListBox != null)
                CriminalsListBox.ItemsSource = CriminalsToShow;
        }

        private void Reload_CriminalGroups()
        {
            if (AddNewCriminal_CriminalGroupComboBox == null) return;
            AddNewCriminal_CriminalGroupComboBox.ItemsSource = Database.CriminalGroups;
            CriminalGroupsListBox.ItemsSource = Database.CriminalGroups;
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
            ///todo:fix govnocode
            List<Language> languages = (from string selectedItem in AddNewCriminal_LanguagesListBox.SelectedItems select new Language(Model.Language.Languages.IndexOf(selectedItem))).ToList();
            ///todo:fix govnocode
            List<Crime> charges = (from string selectedItem in AddNewCriminal_ChargesListBox.SelectedItems select new Crime(Model.Crime.Crimes.IndexOf(selectedItem))).ToList();

            Database.AddCriminal(new Сriminal(
                AddNewCriminal_LastnameTextBox.Text,
                AddNewCriminal_ForenameTextBox.Text,
                AddNewCriminal_CodenameTextBox.Text,
                (AddNewCriminal_IsHeightKnownCheckBox.IsChecked.Value) ? (int?)AddNewCriminal_HeightSlider.Value : null,
                new EyeColor(AddNewCriminal_EyeColorComboBox.SelectedIndex),
                new HairColor(AddNewCriminal_HairColorComboBox.SelectedIndex),
                (Сriminal.SexOptions)AddNewCriminal_SexComboBox.SelectedIndex,
                AddNewCriminal_SpecialSignsTextBox.Text,
                new Country(AddNewCriminal_CitizenshipComboBox.SelectedIndex),
                new Country(AddNewCriminal_BirthCountryComboBox.SelectedIndex),
                AddNewCriminal_BirthplaceTextBox.Text,
                AddNewCriminal_BirthdateDatePicker.SelectedDate,
                new Country(AddNewCriminal_LastLivingCountryComboBox.SelectedIndex),
                AddNewCriminal_LastLivingPlaceTextBox.Text,
                languages,
                (Сriminal.CriminalStateOptions)AddNewCriminal_CurrentStateComboBox.SelectedIndex,
                null,
                (AddNewCriminal_CriminalGroupComboBox.SelectedIndex == -1)
                    ? null
                    : Database.CriminalGroups[((KeyValuePair<int,CriminalGroup>)AddNewCriminal_CriminalGroupComboBox.SelectedItem).Key],
                charges
                )
                );

            if (AddNewCriminal_PhotoFilePath.Text != "")
                Database.ChangeCriminalsPhoto(AddNewCriminal_PhotoFilePath.Text, Сriminal.LastId);

            Reload_CriminalsListBox();
            AddNewCriminalFlyout.IsOpen = false;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Database.SaveData();
        }

        private void CriminalActions_AddCriminalButton_OnClick(object sender, RoutedEventArgs e) => AddNewCriminalFlyout.IsOpen = true;

        private void Flyout_OnIsOpenChanged(object sender, RoutedEventArgs e) => MainGrid.IsEnabled = !MainGrid.IsEnabled;

        private void CriminalActions_DeleteCriminalButton_OnClick(object sender, RoutedEventArgs e)
        {
            Database.DeleteCriminal(((CriminalsListboxItemData)CriminalsListBox.SelectedItem).Id);
            Reload_CriminalsListBox();
        }

        private void CriminalsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            CriminalActions_DeleteCriminalButton.IsEnabled = CriminalsListBox.SelectedItems.Count != 0;
            CriminalActions_OpenProfileButton.IsEnabled = CriminalsListBox.SelectedItems.Count != 0;
        }

        private void Filter_ControlValueChanged(object sender, object e) => Reload_CriminalsListBox();

        private void Filter_ShowAllCriminals_OnClick(object sender, RoutedEventArgs e) => CriminalGroupsListBox.UnselectAll();

        private void CriminalGroupsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CriminalGroupsListBox.SelectedIndex == -1)
            {
                CriminalGroupInformationGrid.Visibility = Visibility.Collapsed;
                CriminalGroupInformationGrid_Name.Content = "";
                CriminalGroupInformationGrid_Information.Text = "";
            }
            else
            {
                var selectedCriminalGroup = ((KeyValuePair<int, CriminalGroup>) CriminalGroupsListBox.SelectedItem).Value;
                CriminalGroupInformationGrid.Visibility = Visibility.Visible;
                CriminalGroupInformationGrid_Name.Content = selectedCriminalGroup.Name;
                CriminalGroupInformationGrid_Information.Text = selectedCriminalGroup.AdditionalData;
            }
            CriminalGroupsListBox_DeleteGroupButton.IsEnabled = CriminalGroupsListBox.SelectedIndex != -1;
            Reload_CriminalsListBox();
        }

        private void CriminalGroupsListBox_AddGroupButton_OnClick(object sender, RoutedEventArgs e) => AddNewCriminalGroupFlyout.IsOpen = true;

        private void AddNewCriminalGroup_SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (AddNewCriminalGroup_NameTextBox.Text == "")
            {
                AddNewCriminalGroup_NameTextBox.Text = "Required";
                return;
            }

            Database.AddCriminalGroup(new CriminalGroup(AddNewCriminalGroup_NameTextBox.Text, AddNewCriminalGroup_AdditionalDataTextBox.Text));
            AddNewCriminalGroupFlyout.IsOpen = false;
            Reload_CriminalGroups();
        }
        
        private void CriminalGroupsListBox_DeleteGroupButton_OnClick(object sender, RoutedEventArgs e) {
            Database.DeleteCriminalGroup(((KeyValuePair<int, CriminalGroup>)CriminalGroupsListBox.SelectedItem).Key);
            Reload_CriminalGroups();
        }

        private void CriminalActions_OpenProfileButton_OnClick(object sender, RoutedEventArgs e)
        {
            CriminalProfile criminalProfile = new CriminalProfile(Database.Criminals[((CriminalsListboxItemData)CriminalsListBox.SelectedItem).Id]);
            criminalProfile.ShowDialog();
            Reload_CriminalsListBox();
        }

        private void CriminalGroupInformationGrid_ChangeDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            CriminalGroup selectedCriminalGroup = ((KeyValuePair<int, CriminalGroup>) CriminalGroupsListBox.SelectedItem).Value;
            ChangeCriminalGroupFlyout_NameTextBox.Text = selectedCriminalGroup.Name;
            ChangeCriminalGroupFlyout_AdditionalDataTextBox.Text = selectedCriminalGroup.AdditionalData;
            ChangeCriminalGroupFlyout.IsOpen = true;
        }

        private void ChangeCriminalGroupFlyout_SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChangeCriminalGroupFlyout_NameTextBox.Text == "")
            {
                ChangeCriminalGroupFlyout_NameTextBox.Text = "Required";
                return;
            }
            CriminalGroup selectedCriminalGroup = ((KeyValuePair<int, CriminalGroup>)CriminalGroupsListBox.SelectedItem).Value;
            selectedCriminalGroup.Name = ChangeCriminalGroupFlyout_NameTextBox.Text;
            selectedCriminalGroup.AdditionalData = ChangeCriminalGroupFlyout_AdditionalDataTextBox.Text;
            ChangeCriminalGroupFlyout.IsOpen = false;
            Reload_CriminalGroups();
            CriminalGroupInformationGrid_Name.Content = selectedCriminalGroup.Name;
            CriminalGroupInformationGrid_Information.Text = selectedCriminalGroup.AdditionalData;
        }

        private void RightWindowCommands_AboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("About this program",
                "Курсовой проект по ООП студента группы ПИ-15-1 ХНУРЭ Слупского Данилы\n\n" +
                "Картотека Интерпола.\n\n"+"Данные по каждому зарегистрированному преступнику: фамилия, имя, кличка, рост, цвет волос, цвет глаз, особые приметы, гражданство, место и дата рождения, последнее место жительства, знание языков, преступная профессия, последнее дело и так далее. Преступные и мафиозные группировки(данные о подельщиках). Выборка по любому подмножеству признаков. Перенос «завязавших» в архив; удаление –только после смерти.",
                MessageDialogStyle.Affirmative);
        }
        
    }
}