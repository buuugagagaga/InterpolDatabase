using System.Windows;

namespace InterpolDatabaseProject
{
    /// <summary>
    /// Логика взаимодействия для DeleteDialog.xaml
    /// Данное окно используется для запроса разрешения на удаление данных
    /// </summary>
    public partial class DeleteDialog
    {
        #region Properties
        /// <summary>
        /// Ответ пользователя
        /// </summary>
        public bool Result { get; private set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Основной конструктор
        /// </summary>
        public DeleteDialog()
        {
            InitializeComponent();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Закрывает окно с положительным ответом
        /// </summary>
        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }
        /// <summary>
        /// Закрывает окно с отрицательным ответом
        /// </summary>
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
        #endregion
    }
}
