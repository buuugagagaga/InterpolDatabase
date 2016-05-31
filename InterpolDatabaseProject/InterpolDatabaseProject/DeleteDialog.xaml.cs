using System.Windows;

namespace InterpolDatabaseProject
{
    /// <summary>
    /// Логика взаимодействия для DeleteDialog.xaml
    /// </summary>
    public partial class DeleteDialog
    {
        public bool Result { get; private set; }

        public DeleteDialog()
        {
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }
    }
}
