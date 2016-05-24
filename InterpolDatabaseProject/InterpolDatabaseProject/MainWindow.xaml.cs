using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            Сriminal criminal = new Сriminal("Lastname", "Forename", "Codename", 100, new EyeColor(0), new HairColor(0), Сriminal.SexOptions.Unknown, new List<string> {"Something"}, new Country(0), new Country(0), "Unknown", DateTime.MaxValue, new Country(0), "Unknown",new List<Language> {new Language(0)});
            Crime crime = new Crime(new Crime.CrimeType(0), DateTime.MaxValue, new Country(0), "Unknown", "No data");
            CriminalGroup criminalGroup = new CriminalGroup("Cosa nostra", "No data");
            
        }
    }
}
