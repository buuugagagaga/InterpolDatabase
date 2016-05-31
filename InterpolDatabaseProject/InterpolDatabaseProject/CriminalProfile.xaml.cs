using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using InterpolDatabaseProject.Model;
using Microsoft.CSharp.RuntimeBinder;

namespace InterpolDatabaseProject
{
    /// <summary>
    /// Логика взаимодействия для CriminalProfile.xaml
    /// </summary>
    public partial class CriminalProfile
    {
        private readonly Сriminal _criminal;

        public CriminalProfile(Сriminal criminal)
        {
            InitializeComponent();
            _criminal = criminal;
            LoadInformation();
            SetDataSources();
        }

        public void LoadInformation()
        {

            if (_criminal == null) return;
            const string unknown = "Unknown";
            const string nodata = "No data";

            Title = "Criminal #" + _criminal.Id;
            if (_criminal.PhotoFileName != null) CriminalImage.Source = new BitmapImage(new Uri(Path.GetFullPath("..\\..\\Storage\\Files\\" + _criminal.PhotoFileName)));

            ForenameLabel.Content = (_criminal.Forename=="")? unknown : _criminal.Forename;
            CodenameLabel.Content = (_criminal.CodeName == "") ? unknown : _criminal.CodeName;
            LastnameLabel.Content = (_criminal.Lastname == "") ? unknown : _criminal.Lastname;
            StateLabel.Content = _criminal.State;
            HeightLabel.Content = (_criminal.Height==null)? unknown : _criminal.Height.ToString();
            AgeLabel.Content = (_criminal.Age == null) ? unknown : _criminal.Age.ToString();
            CriminalGroupLabel.Content = _criminal.CriminalGroupMembership?.ToString() ?? "Singleton";
            ColorOfEyesLabel.Content = _criminal.ColorOfEye;
            ColorOfHairLabel.Content = _criminal.ColorOfHair;
            SexLabel.Content = _criminal.Sex;
            LanguagesTextBlock.Text = _criminal.Languages.Count > 0 ? 
                _criminal.Languages.Aggregate("", (current, language) => current + language + " ") 
                :
                nodata;
            ChargesTextBlock.Text = _criminal.Charges.Count > 0 ?
                _criminal.Charges.Aggregate("", (current, charge) => current + charge + " ")
                :
                nodata;
            CitizenshipLabel.Content = _criminal.Citizenship;
            CountryOfBirthLabel.Content = _criminal.BirthCountry;
            PlaceOfBirthLabel.Content = (_criminal.Birthplace=="")?unknown:_criminal.Birthplace;
            DateOfBirthLabel.Content = _criminal.Birthdate?.ToShortDateString() ?? unknown;
            LastLivedInLabel.Content = _criminal.LastLivingCountry;
            LastPlaceOfLiving.Content = (_criminal.LastLivingPlace == "") ? unknown : _criminal.LastLivingPlace;
            SpecialSignsTextBlock.Text = (_criminal.SpecialSigns == "") ? nodata : _criminal.SpecialSigns;
        }
        
        private void ChangeCriminalFlyout_SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<Сriminal.Language> languages = ChangeCriminalFlyout_LanguagesListBox.SelectedItems.Cast<Сriminal.Language>().ToList();
            List<Сriminal.Crime> charges = ChangeCriminalFlyout_ChargesListBox.SelectedItems.Cast<Сriminal.Crime>().ToList();

            _criminal.Lastname = ChangeCriminalFlyout_LastnameTextBox.Text;
            _criminal.Forename = ChangeCriminalFlyout_ForenameTextBox.Text;
            _criminal.CodeName = ChangeCriminalFlyout_CodenameTextBox.Text;
            _criminal.Height = (ChangeCriminalFlyout_IsHeightKnownCheckBox.IsChecked.Value) ? (int?) ChangeCriminalFlyout_HeightSlider.Value : null;
            _criminal.ColorOfEye = (Сriminal.EyeColor)ChangeCriminalFlyout_EyeColorComboBox.SelectedItem;
            _criminal.ColorOfHair = (Сriminal.HairColor)ChangeCriminalFlyout_HairColorComboBox.SelectedItem;
            _criminal.Sex = (Сriminal.SexOptions) ChangeCriminalFlyout_SexComboBox.SelectedItem;
            _criminal.SpecialSigns = ChangeCriminalFlyout_SpecialSignsTextBox.Text;
            _criminal.Citizenship = (Сriminal.Country)ChangeCriminalFlyout_CitizenshipComboBox.SelectedItem;
            _criminal.BirthCountry = (Сriminal.Country)ChangeCriminalFlyout_BirthCountryComboBox.SelectedItem;
            _criminal.Birthplace = ChangeCriminalFlyout_BirthplaceTextBox.Text;
            _criminal.Birthdate = ChangeCriminalFlyout_BirthdateDatePicker.SelectedDate;
            _criminal.LastLivingCountry = (Сriminal.Country)ChangeCriminalFlyout_LastLivingCountryComboBox.SelectedIndex;
            _criminal.LastLivingPlace = ChangeCriminalFlyout_LastLivingPlaceTextBox.Text;
            _criminal.Languages = languages;
            _criminal.Charges = charges;
            _criminal.State = (Сriminal.CriminalStateOptions) ChangeCriminalFlyout_CurrentStateComboBox.SelectedIndex;
            _criminal.UnsetCriminalGroup();
            if(ChangeCriminalFlyout_CriminalGroupComboBox.SelectedIndex != -1)
                _criminal.SetCriminalGroup(Database.CriminalGroups[
                    ((KeyValuePair<int, CriminalGroup>) ChangeCriminalFlyout_CriminalGroupComboBox.SelectedItem).Key]);

            if (ChangeCriminalFlyout_PhotoFilePath.Text != "")
                Database.ChangeCriminalsPhoto(ChangeCriminalFlyout_PhotoFilePath.Text, _criminal.Id);
            
            LoadInformation();
            ChangeCriminalFlyout.IsOpen = false;
        }

        private void ChangeCriminalFlyout_PickButton_OnClick(object sender, RoutedEventArgs e)
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
                ChangeCriminalFlyout_PhotoFilePath.Text = dlg.FileName;
            }
        }

        private void ChangeCriminalFlyout_OnIsOpenChanged(object sender, RoutedEventArgs e)
        {
            MainGrid.IsEnabled = !ChangeCriminalFlyout.IsOpen;
            if (ChangeCriminalFlyout.IsOpen)
                LoadChangeCriminalData();
        }

        private void LoadChangeCriminalData()
        {
            ChangeCriminalFlyout_ForenameTextBox.Text = _criminal.Forename;
            ChangeCriminalFlyout_CodenameTextBox.Text = _criminal.CodeName;
            ChangeCriminalFlyout_LastnameTextBox.Text = _criminal.Lastname;
            ChangeCriminalFlyout_HeightSlider.Value = _criminal.Height ?? 0;
            ChangeCriminalFlyout_EyeColorComboBox.SelectedIndex = (int)_criminal.ColorOfEye;
            ChangeCriminalFlyout_HairColorComboBox.SelectedIndex = (int)_criminal.ColorOfHair;
            ChangeCriminalFlyout_SexComboBox.SelectedIndex = (int)_criminal.Sex;
            ChangeCriminalFlyout_CitizenshipComboBox.SelectedIndex = (int)_criminal.Citizenship;
            ChangeCriminalFlyout_BirthCountryComboBox.SelectedIndex = (int)_criminal.BirthCountry;
            ChangeCriminalFlyout_BirthplaceTextBox.Text = _criminal.Birthplace;
            ChangeCriminalFlyout_BirthdateDatePicker.SelectedDate = _criminal.Birthdate;
            ChangeCriminalFlyout_LastLivingCountryComboBox.SelectedIndex = (int)_criminal.LastLivingCountry;
            ChangeCriminalFlyout_LastLivingPlaceTextBox.Text = _criminal.LastLivingPlace;
            ChangeCriminalFlyout_CurrentStateComboBox.SelectedIndex = (int)_criminal.State;
            ChangeCriminalFlyout_SpecialSignsTextBox.Text = _criminal.SpecialSigns;
            foreach (var language in _criminal.Languages)
                ChangeCriminalFlyout_LanguagesListBox.SelectedItems.Add(ChangeCriminalFlyout_LanguagesListBox.Items[(int)language]);
            foreach (var charge in _criminal.Charges)
                ChangeCriminalFlyout_ChargesListBox.SelectedItems.Add(ChangeCriminalFlyout_ChargesListBox.Items[(int)charge]);
            if (_criminal.CriminalGroupMembership == null) return;
            for (int i = 0; i < Database.CriminalGroups.Values.Count; i++)
            {
                if (_criminal.CriminalGroupMembership.Id != Database.CriminalGroups.Values.ElementAt(i).Id) continue;
                ChangeCriminalFlyout_CriminalGroupComboBox.SelectedIndex = i;
                break;
            }
        }

        private void SetDataSources()
        {
            ChangeCriminalFlyout_EyeColorComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.EyeColor)).Cast<Сriminal.EyeColor>();
            ChangeCriminalFlyout_HairColorComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.HairColor)).Cast<Сriminal.HairColor>();
            ChangeCriminalFlyout_SexComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.SexOptions)).Cast<Сriminal.SexOptions>();
            ChangeCriminalFlyout_CitizenshipComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.Country)).Cast<Сriminal.Country>();
            ChangeCriminalFlyout_BirthCountryComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.Country)).Cast<Сriminal.Country>();
            ChangeCriminalFlyout_LastLivingCountryComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.Country)).Cast<Сriminal.Country>();
            ChangeCriminalFlyout_CurrentStateComboBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.CriminalStateOptions)).Cast<Сriminal.CriminalStateOptions>();
            ChangeCriminalFlyout_LanguagesListBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.Language)).Cast<Сriminal.Language>();
            ChangeCriminalFlyout_ChargesListBox.ItemsSource =
                Enum.GetValues(typeof(Сriminal.Crime)).Cast<Сriminal.Crime>();
            ChangeCriminalFlyout_CriminalGroupComboBox.ItemsSource = Database.CriminalGroups;
        }

        private void ChangeProfileButton_OnClick(object sender, RoutedEventArgs e) => ChangeCriminalFlyout.IsOpen = true;
    }
}
