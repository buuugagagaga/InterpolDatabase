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
    public partial class MainWindow
    {
        #region Constructors
        /// <summary>
        /// Основной конструктор
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Database.RestoreData();
            InitializeComponent();
            SetDataSources();
            Reload_CriminalsListBox();
            Reload_CriminalGroups();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Отфильтрованная коллекция преступников
        /// </summary>
        public List<Сriminal> FilteredСriminals => ApplyFiltersToCriminals(Database.Criminals);

        /// <summary>
        /// Коллекция объектов CriminalToShow, являющаяся источником данных при отображении списка преступников
        /// </summary>
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
        #endregion

        #region Methods 

        /// <summary>
        /// Применяет необходимые фильтры к коллекции преступников
        /// </summary>
        /// <param name="criminals">Полная коллекция преступников</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        private List<Сriminal> ApplyFiltersToCriminals(ReadOnlyDictionary<int, Сriminal> criminals)
        {
            var result = CriminalGroupsListBox.SelectedItems.Count == 0 ? new List<Сriminal>(criminals.Values) : new List<Сriminal>(((KeyValuePair<int, CriminalGroup>)CriminalGroupsListBox.SelectedItem).Value.Members.Values);

            if (Filter_StateListBox.SelectedItems.Count > 0) result = Filter.FilterByState(result, Filter_StateListBox.SelectedItems);

            if (Filter_ForenameTextBox.Text != "") result = Filter.FilterByForename(result, Filter_ForenameTextBox.Text);
            if (Filter_CodenameTextBox.Text != "") result = Filter.FilterByCodename(result, Filter_CodenameTextBox.Text);
            if (Filter_LastnameTextBox.Text != "") result = Filter.FilterByLastname(result, Filter_LastnameTextBox.Text);

            if (!Filter_IsAgeKnown.IsChecked.Value)
                result = Filter.FilterByAge(result);
            else if (Filter_AgeRangeSlider.LowerValue > Filter_AgeRangeSlider.Minimum || Filter_AgeRangeSlider.UpperValue < Filter_AgeRangeSlider.Maximum)
                result = Filter.FilterByAge(result, (int)Filter_AgeRangeSlider.LowerValue, (int)Filter_AgeRangeSlider.UpperValue);

            if (!Filter_IsHeightKnown.IsChecked.Value)
                result = Filter.FilterByHeight(result);
            else if (Filter_HeightRangeSlider.LowerValue > Filter_HeightRangeSlider.Minimum || Filter_HeightRangeSlider.UpperValue < Filter_HeightRangeSlider.Maximum)
                result = Filter.FilterByHeight(result, (int)Filter_HeightRangeSlider.LowerValue, (int)Filter_HeightRangeSlider.UpperValue);

            if (Filter_EyeColorCombobox.SelectedIndex != -1)
                result = Filter.FilterByEyeColor(result, (EyeColor)Filter_EyeColorCombobox.SelectedItem);
            if (Filter_HairColorCombobox.SelectedIndex != -1)
                result = Filter.FilterByHairColor(result, (HairColor)Filter_HairColorCombobox.SelectedItem);
            if (Filter_SexCombobox.SelectedIndex != -1)
                result = Filter.FilterBySex(result, (SexOptions)Filter_SexCombobox.SelectedItem);
            if (Filter_SpecialSignsTextbox.Text != "")
                result = Filter.FilterBySpecialSigns(result, Filter_SpecialSignsTextbox.Text);
            if (Filter_LanguagesListbox.SelectedItems.Count > 0)
                result = Filter.FilterByLanguages(result, Filter_LanguagesListbox.SelectedItems);
            if (Filter_CitizenshipComboBox.SelectedIndex != -1)
                result = Filter.FilterByCitizenship(result, (Country)Filter_CitizenshipComboBox.SelectedItem);
            if (Filter_BirthCountryComboBox.SelectedIndex != -1)
                result = Filter.FilterByBirthCountry(result, (Country)Filter_BirthCountryComboBox.SelectedItem);
            if (Filter_PlaceOfBirthTextBox.Text != "")
                result = Filter.FilterByBirthPlace(result, Filter_PlaceOfBirthTextBox.Text);
            if (Filter_LastLivingCountryComboBox.SelectedIndex != -1)
                result = Filter.FilterByLastLivingCountry(result, (Country)Filter_LastLivingCountryComboBox.SelectedItem);
            if (Filter_LastLivingPlaceTextBox.Text != "")
                result = Filter.FilterByLastLivingPlace(result, Filter_LastLivingPlaceTextBox.Text);

            return result;
        }

        /// <summary>
        /// Устанавливает источники неизменяемых в программе данных для соответствующих элементов управления
        /// </summary>
        private void SetDataSources()
        {
            AddNewCriminal_EyeColorComboBox.ItemsSource =
                Enum.GetValues(typeof(EyeColor)).Cast<EyeColor>();
            AddNewCriminal_HairColorComboBox.ItemsSource =
                Enum.GetValues(typeof(HairColor)).Cast<HairColor>();
            AddNewCriminal_SexComboBox.ItemsSource =
                Enum.GetValues(typeof(SexOptions)).Cast<SexOptions>();
            AddNewCriminal_CitizenshipComboBox.ItemsSource =
                Enum.GetValues(typeof(Country)).Cast<Country>();
            AddNewCriminal_BirthCountryComboBox.ItemsSource =
                Enum.GetValues(typeof(Country)).Cast<Country>();
            AddNewCriminal_LastLivingCountryComboBox.ItemsSource =
                Enum.GetValues(typeof(Country)).Cast<Country>();
            AddNewCriminal_CurrentStateComboBox.ItemsSource =
                Enum.GetValues(typeof(CriminalStateOptions)).Cast<CriminalStateOptions>();
            AddNewCriminal_LanguagesListBox.ItemsSource =
                Enum.GetValues(typeof(Language)).Cast<Language>();
            AddNewCriminal_ChargesListBox.ItemsSource =
                Enum.GetValues(typeof(Crime)).Cast<Crime>(); ;

            Filter_EyeColorCombobox.ItemsSource =
                Enum.GetValues(typeof(EyeColor)).Cast<EyeColor>();
            Filter_HairColorCombobox.ItemsSource =
                Enum.GetValues(typeof(HairColor)).Cast<HairColor>();
            Filter_SexCombobox.ItemsSource =
                Enum.GetValues(typeof(SexOptions)).Cast<SexOptions>();
            Filter_LanguagesListbox.ItemsSource =
                Enum.GetValues(typeof(Language)).Cast<Language>();
            Filter_CitizenshipComboBox.ItemsSource =
                Enum.GetValues(typeof(Country)).Cast<Country>();
            Filter_BirthCountryComboBox.ItemsSource =
                Enum.GetValues(typeof(Country)).Cast<Country>();
            Filter_LastLivingCountryComboBox.ItemsSource =
                Enum.GetValues(typeof(Country)).Cast<Country>();
            Filter_StateListBox.ItemsSource =
                Enum.GetValues(typeof(CriminalStateOptions)).Cast<CriminalStateOptions>();
        }

        /// <summary>
        /// Перезагружает источники данных в элементах управления, отображающих преступников
        /// </summary>
        private void Reload_CriminalsListBox()
        {
            if (CriminalsListBox != null)
                CriminalsListBox.ItemsSource = CriminalsToShow;
        }

        /// <summary>
        /// Перезагружает источники данных в элементах управления, отображающих группировки
        /// </summary>
        private void Reload_CriminalGroups()
        {
            if (AddNewCriminal_CriminalGroupComboBox == null) return;
            AddNewCriminal_CriminalGroupComboBox.ItemsSource = Database.CriminalGroups;
            CriminalGroupsListBox.ItemsSource = Database.CriminalGroups;
        }

        /// <summary>
        /// Обработчик события Click для AddNewCriminal_PickButton.
        /// Выбирает файл с фотографией преступника
        /// </summary>
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

        /// <summary>
        /// Обработчик события Click для AddNewCriminal_SubmitButton.
        /// Создает и помещает в коллекцию новый экземпляр класса Criminal на основе введенных пользователем данных
        /// </summary>
        private void AddNewCriminal_SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            List<Language> languages = AddNewCriminal_LanguagesListBox.SelectedItems.Cast<Language>().ToList();
            List<Crime> charges = AddNewCriminal_ChargesListBox.SelectedItems.Cast<Crime>().ToList();

            Database.AddCriminal(new Сriminal(
                AddNewCriminal_LastnameTextBox.Text,
                AddNewCriminal_ForenameTextBox.Text,
                AddNewCriminal_CodenameTextBox.Text,
                AddNewCriminal_IsHeightKnownCheckBox.IsChecked.Value ? (int?)AddNewCriminal_HeightSlider.Value : null,
                (EyeColor)AddNewCriminal_EyeColorComboBox.SelectedItem,
                (HairColor)AddNewCriminal_HairColorComboBox.SelectedItem,
                (SexOptions)AddNewCriminal_SexComboBox.SelectedItem,
                AddNewCriminal_SpecialSignsTextBox.Text,
                (Country)AddNewCriminal_CitizenshipComboBox.SelectedItem,
                (Country)AddNewCriminal_BirthCountryComboBox.SelectedItem,
                AddNewCriminal_BirthplaceTextBox.Text,
                AddNewCriminal_BirthdateDatePicker.SelectedDate,
                (Country)AddNewCriminal_LastLivingCountryComboBox.SelectedItem,
                AddNewCriminal_LastLivingPlaceTextBox.Text,
                languages,
                (CriminalStateOptions)AddNewCriminal_CurrentStateComboBox.SelectedItem,
                null,
                (AddNewCriminal_CriminalGroupComboBox.SelectedIndex == -1)
                    ? null
                    : Database.CriminalGroups[((KeyValuePair<int, CriminalGroup>)
                        AddNewCriminal_CriminalGroupComboBox.SelectedItem).Key],
                charges
                )
                );

            if (AddNewCriminal_PhotoFilePath.Text != "")
                Database.ChangeCriminalsPhoto(AddNewCriminal_PhotoFilePath.Text, Сriminal.LastId);

            Reload_CriminalsListBox();
            AddNewCriminalFlyout.IsOpen = false;
        }

        /// <summary>
        /// Обработчик события Closing для MainWindow.
        /// Сохраняет данные
        /// </summary>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e) => Database.SaveData();

        /// <summary>
        /// Обработчик события Click для CriminalActions_AddCriminalButton.
        /// Вызывает функцию очистки AddCriminalFlyout и отображает пользователю поля для ввода данных о преступнике 
        /// </summary>
        private void CriminalActions_AddCriminalButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearAddCriminalFlyout();
            AddNewCriminalFlyout.IsOpen = true;
        }

        /// <summary>
        /// Метод очистки полей AddCriminalFlyout от данных предыдущих использований
        /// </summary>
        private void ClearAddCriminalFlyout()
        {
            AddNewCriminal_BirthdateDatePicker.SelectedDate = null;
            AddNewCriminal_LanguagesListBox.UnselectAll();
            AddNewCriminal_BirthCountryComboBox.SelectedIndex = 0;
            AddNewCriminal_BirthplaceTextBox.Text = "";
            AddNewCriminal_ChargesListBox.UnselectAll();
            AddNewCriminal_CitizenshipComboBox.SelectedIndex = 0;
            AddNewCriminal_CodenameTextBox.Text = "";
            AddNewCriminal_CriminalGroupComboBox.SelectedIndex = -1;
            AddNewCriminal_CurrentStateComboBox.SelectedIndex = 0;
            AddNewCriminal_EyeColorComboBox.SelectedIndex = 0;
            AddNewCriminal_HairColorComboBox.SelectedIndex = 0;
            AddNewCriminal_ForenameTextBox.Text = "";
            AddNewCriminal_LastnameTextBox.Text = "";
            AddNewCriminal_SpecialSignsTextBox.Text = "";
            AddNewCriminal_LastLivingCountryComboBox.SelectedIndex = 0;
            AddNewCriminal_LastLivingPlaceTextBox.Text = "";
            AddNewCriminal_SexComboBox.SelectedIndex = 0;
            AddNewCriminal_PhotoFilePath.Text = "";
        }

        /// <summary>
        /// Обработчик события IsOpenChanged для всех элементов Flyout
        /// Блокирует (разблокирует) остальные элементы, во время (после) использования Flyout
        /// </summary>
        private void Flyout_OnIsOpenChanged(object sender, RoutedEventArgs e)
            => MainGrid.IsEnabled = !MainGrid.IsEnabled;

        /// <summary>
        /// Обработчик события Click для CriminalActions_DeleteCriminalButton
        /// Спрашивает разрешение на удаление преступника и в случае подтверждения удаляет выбранного преступника
        /// </summary>
        private void CriminalActions_DeleteCriminalButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!AskPermissionToDelete()) return;
            Database.DeleteCriminal(((CriminalsListboxItemData)CriminalsListBox.SelectedItem).Id);
            Reload_CriminalsListBox();
        }

        /// <summary>
        /// Обработчик события SelectionChanged для CriminalsListBox
        /// Блокирует/разблокирует кнопки открытия профиля и удаления преступника
        /// </summary>
        private void CriminalsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CriminalActions_DeleteCriminalButton.IsEnabled = CriminalsListBox.SelectedItems.Count != 0;
            CriminalActions_OpenProfileButton.IsEnabled = CriminalsListBox.SelectedItems.Count != 0;
        }

        /// <summary>
        /// Обработчик событий измения данных для всех элементов управления фильтра
        /// Перезагружает список преступников при изменении значений полей фильтра
        /// </summary>
        private void Filter_ControlValueChanged(object sender, object e) => Reload_CriminalsListBox();

        /// <summary>
        /// Обработчик события Click для Filter_ShowAllCriminals
        /// Сбрасывает выбор группировки
        /// </summary>
        private void Filter_ShowAllCriminals_OnClick(object sender, RoutedEventArgs e)
            => CriminalGroupsListBox.UnselectAll();

        /// <summary>
        /// Обработчик события SelectionChanged для CriminalGroupsListBox
        /// Отображает/скрывает данные о группировке и разблокирует/блокирует кнопку удаления
        /// </summary>
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
                var selectedCriminalGroup = ((KeyValuePair<int, CriminalGroup>)CriminalGroupsListBox.SelectedItem).Value;
                CriminalGroupInformationGrid.Visibility = Visibility.Visible;
                CriminalGroupInformationGrid_Name.Content = selectedCriminalGroup.Name;
                CriminalGroupInformationGrid_Information.Text = selectedCriminalGroup.AdditionalData;
            }
            CriminalGroupsListBox_DeleteGroupButton.IsEnabled = CriminalGroupsListBox.SelectedIndex != -1;
            Reload_CriminalsListBox();
        }

        /// <summary>
        /// Обработчик события Click для CriminalGroupsListBox_AddGroupButton
        /// Очищает и открывает  Flyout для добавления новой группировки
        /// </summary>
        private void CriminalGroupsListBox_AddGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearAddNewCriminalGroupFlyout();
            AddNewCriminalGroupFlyout.IsOpen = true;
        }

        /// <summary>
        /// Очищает значения элементов управления внутри AddNewCriminalGroupFlyout
        /// </summary>
        private void ClearAddNewCriminalGroupFlyout()
        {
            AddNewCriminalGroup_NameTextBox.Text = "";
            AddNewCriminalGroup_AdditionalDataTextBox.Text = "";
        }

        /// <summary>
        /// Обработчик события Click для AddNewCriminalGroup_SubmitButton
        /// Проверяет заполненные пользователем поля при добавлении группировки
        /// В случае успешного заполнения создает новую группировку
        /// </summary>
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

        /// <summary>
        /// Обработчик события Click для CriminalGroupsListBox_DeleteGroupButton
        /// Удаляет выбранную группировку
        /// </summary>
        private void CriminalGroupsListBox_DeleteGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!AskPermissionToDelete()) return;
            Database.DeleteCriminalGroup(((KeyValuePair<int, CriminalGroup>)CriminalGroupsListBox.SelectedItem).Key);
            Reload_CriminalGroups();
        }

        /// <summary>
        /// Обработчик события Click для CriminalActions_OpenProfileButton
        /// Открывает профиль выбранного преступника
        /// </summary>
        private void CriminalActions_OpenProfileButton_OnClick(object sender, RoutedEventArgs e)
        {
            CriminalProfile criminalProfile = new CriminalProfile(Database.Criminals[((CriminalsListboxItemData)CriminalsListBox.SelectedItem).Id]);

            criminalProfile.ShowDialog();
            Reload_CriminalsListBox();
        }

        /// <summary>
        /// Обработчик события Click для CriminalGroupInformationGrid_ChangeDataButton
        /// Открывает Flyout для изменения данных о группировке
        /// </summary>
        private void CriminalGroupInformationGrid_ChangeDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            CriminalGroup selectedCriminalGroup = ((KeyValuePair<int, CriminalGroup>)CriminalGroupsListBox.SelectedItem).Value;
            ChangeCriminalGroupFlyout_NameTextBox.Text = selectedCriminalGroup.Name;
            ChangeCriminalGroupFlyout_AdditionalDataTextBox.Text = selectedCriminalGroup.AdditionalData;
            ChangeCriminalGroupFlyout.IsOpen = true;
        }

        /// <summary>
        /// Обработчик события Click для ChangeCriminalGroupFlyout_SubmitButton
        /// Подтверждает внесенные в информацию о группировке изменения
        /// </summary>
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

        /// <summary>
        /// Обработчик события Click для RightWindowCommands_AboutButton
        /// Отображает информацию о данной программе
        /// </summary>
        private void RightWindowCommands_AboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("About this program",
                "Курсовой проект по ООП студента группы ПИ-15-1 ХНУРЭ Слупского Данилы\n\n" +
                "Картотека Интерпола.\n\n" + "Данные по каждому зарегистрированному преступнику: фамилия, имя, кличка, рост, цвет волос, цвет глаз, особые приметы, гражданство, место и дата рождения, последнее место жительства, знание языков, преступная профессия, последнее дело и так далее. Преступные и мафиозные группировки(данные о подельщиках). Выборка по любому подмножеству признаков. Перенос «завязавших» в архив; удаление –только после смерти.",
                MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// Обработчик события Click для RightWindowCommands_SaveButton
        /// Сохраняет текущее состояние базы в файл
        /// </summary>
        private void RightWindowCommands_SaveButton_OnClick(object sender, RoutedEventArgs e)
            => Database.SaveData();

        /// <summary>
        /// Обработчик события Click для RightWindowCommands_RestoreButton
        /// Восстанавливает последнее состояние базы из файла
        /// </summary>
        private void RightWindowCommands_RestoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            Database.RestoreData();
            Reload_CriminalsListBox();
            Reload_CriminalGroups();
        }

        /// <summary>
        /// Требует от пользователя разрешение на удаление данных
        /// </summary>
        /// <returns>Имеет ли пользователь разрешение на удаление данных</returns>
        private bool AskPermissionToDelete()
        {
            DeleteDialog dialog = new DeleteDialog();
            dialog.ShowDialog();
            return dialog.Result;
        }
        #endregion
    }
    
    /// <summary>
    /// Класс, используемый при отображении краткой информации о преступнике
    /// </summary>
    public class CriminalsListboxItemData
    {
        #region Constructors
        /// <summary>
        /// Основной конструктор
        /// </summary>
        /// <param name="id">Уникальный номер преступника</param>
        /// <param name="forename">Имя преступника</param>
        /// <param name="codename">Кличка преступника</param>
        /// <param name="lastname">Фамилия преступника</param>
        /// <param name="age">Возраст преступника</param>
        /// <param name="citizenship">Гражданство преступника</param>
        /// <param name="imageName">Название файла с фото преступника</param>
        /// <param name="state">Текущий статус преступника</param>
        public CriminalsListboxItemData(int id, string forename, string codename, string lastname, int? age, string citizenship, string imageName, CriminalStateOptions state)
        {
            Id = id;
            TotalName = "#" + id + " - " + forename + " \"" + codename + "\" " + lastname + " AGE: " + ((age == null) ? "-" : age.ToString());
            Citizenship = citizenship;
            if (!File.Exists("..\\..\\Storage\\Files\\" + imageName))
                throw new FileNotFoundException();
            Image = new BitmapImage(new Uri(Path.GetFullPath("..\\..\\Storage\\Files\\" + imageName)));

            State = state;
            switch (state)
            {
                case CriminalStateOptions.Busted:
                    TextColor = Brushes.Yellow;
                    break;
                case CriminalStateOptions.Wasted:
                    TextColor = Brushes.Red;
                    break; ;
                case CriminalStateOptions.Wanted:
                    TextColor = Brushes.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        #endregion
        #region Properties
        /// <summary>
        /// Уникальный номер преступника
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Отображаемое полное имя преступника
        /// </summary>
        public string TotalName { get; }

        /// <summary>
        /// Гражданство преступника
        /// </summary>
        public string Citizenship { get; set; }

        /// <summary>
        /// Изображение преступника
        /// </summary>
        public BitmapImage Image { get; set; }

        /// <summary>
        /// Цвет надписи статуса преступника
        /// Wanted - белый, Busted - желтый, Wasted - красный
        /// </summary>
        public Brush TextColor { get; set; }

        /// <summary>
        /// Текущий статус преступника
        /// </summary>
        public CriminalStateOptions State { get; set; }
        #endregion
    }

    /// <summary>
    /// Статический класс для группировки методов фильтрации данных
    /// </summary>
    public static class Filter
    {
        /// <summary>
        /// Фильтрация по статусу преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedItems">Выбранные значения</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByState(List<Сriminal> result, IList selectedItems)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (selectedItems.Cast<CriminalStateOptions>()
                    .Any(selectedItem => result[i].State == selectedItem)) continue;
                result.RemoveAt(i);
                i--;
            }
            return result;
        }

        /// <summary>
        /// Фильтрация по имени преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="text">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByForename(List<Сriminal> result, string text)
            => result.Where(r => r.Forename.ToLower().Contains(text.ToLower())).ToList();

        /// <summary>
        /// Фильтрация по кличке преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="text">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByCodename(List<Сriminal> result, string text)
            => result.Where(r => r.CodeName.ToLower().Contains(text.ToLower())).ToList();

        /// <summary>
        /// Фильтрация по фамилии преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="text">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByLastname(List<Сriminal> result, string text)
            => result.Where(r => r.Lastname.ToLower().Contains(text.ToLower())).ToList();

        /// <summary>
        /// Фильтрация по диапазону возрастов преступника (если возраст известен)
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="lowerValue">Начальный возраст</param>
        /// <param name="upperValue">Конечный возраст</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByAge(List<Сriminal> result, int lowerValue, int upperValue)
            => result.Where(r => r.Age >= lowerValue && r.Age <= upperValue).ToList();

        /// <summary>
        /// Фильтрация по диапазону возрастов преступника (если возраст неизвестен)
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByAge(List<Сriminal> result)
            => result.Where(r => r.Age == null).ToList();

        /// <summary>
        /// Фильтрация по диапазону роста преступника (если рост известен)
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="lowerValue">Начальный рост</param>
        /// <param name="upperValue">Конечный рост</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByHeight(List<Сriminal> result, int lowerValue, int upperValue)
            => result.Where(r => r.Height >= lowerValue && r.Height <= upperValue).ToList();

        /// <summary>
        /// Фильтрация по диапазону роста преступника (если рост неизвестен)
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByHeight(List<Сriminal> result)
            => result.Where(r => r.Height == null).ToList();

        /// <summary>
        /// Фильтрация по цвету глаз преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedValue">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByEyeColor(List<Сriminal> result, EyeColor selectedValue)
            => result.Where(r => r.ColorOfEye == selectedValue).ToList();

        /// <summary>
        /// Фильтрация по цвету волос преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedValue">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByHairColor(List<Сriminal> result, HairColor selectedValue)
            => result.Where(r => r.ColorOfHair == selectedValue).ToList();

        /// <summary>
        /// Фильтрация по полу преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedValue">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterBySex(List<Сriminal> result, SexOptions selectedValue)
            => result.Where(r => r.Sex == selectedValue).ToList();
        
        /// <summary>
        /// Фильтрация по особым приметам преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="text">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterBySpecialSigns(List<Сriminal> result, string text)
            => result.Where(r => r.SpecialSigns.ToLower().Contains(text.ToLower())).ToList();

        /// <summary>
        /// Фильтрация по языкам, которые знает преступник
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedItems">Выбранные значения</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByLanguages(List<Сriminal> result, IList selectedItems)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (selectedItems.Cast<Language>().All(
                    selectedItem => result[i].Languages.Contains(selectedItem))) continue;
                result.RemoveAt(i);
                i--;
            }
            return result;
        }

        /// <summary>
        /// Фильтрация по гражданству преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedValue">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByCitizenship(List<Сriminal> result, Country selectedValue) =>
            result.Where(r => r.Citizenship == selectedValue).ToList();

        /// <summary>
        /// Фильтрация по стране рождения преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedValue">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByBirthCountry(List<Сriminal> result, Country selectedValue) =>
            result.Where(r => r.BirthCountry == selectedValue).ToList();

        /// <summary>
        /// Фильтрация по месту рождения преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="text">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByBirthPlace(List<Сriminal> result, string text) =>
            result.Where(r => r.Birthplace.ToLower().Contains(text.ToLower())).ToList();

        /// <summary>
        /// Фильтрация по последней зарегистрированной стране проживания преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="selectedValue">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByLastLivingCountry(List<Сriminal> result, Country selectedValue) =>
            result.Where(r => r.LastLivingCountry == selectedValue).ToList();

        /// <summary>
        /// Фильтрация по последнему зарегистрированному месту проживания преступника
        /// </summary>
        /// <param name="result">Результат предыдущих фильтраций</param>
        /// <param name="text">Выбранное значение</param>
        /// <returns>Отфильтрованная коллекция преступников</returns>
        public static List<Сriminal> FilterByLastLivingPlace(List<Сriminal> result, string text) =>
            result.Where(r => r.LastLivingPlace.ToLower().Contains(text.ToLower())).ToList();
    }
}