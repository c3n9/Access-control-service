using AccessControlService.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AccessControlService.Pages
{
    /// <summary>
    /// Логика взаимодействия для PAccessControl.xaml
    /// </summary>
    public partial class PAccessControl : Page
    {
        User contextUser = new User();
        public PAccessControl()
        {
            InitializeComponent();
            DataContext = contextUser;
            CBGender.ItemsSource = App.DB.Gender.ToList();
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string error = "";
            if (String.IsNullOrWhiteSpace(contextUser.Surname))
            {
                error += "Введите фамилию\n";
            }
            if (String.IsNullOrWhiteSpace(contextUser.Name))
            {
                error += "Введите имя\n";
            }
            if (String.IsNullOrWhiteSpace(contextUser.Patronymic))
            {
                error += "Введите отчество\n";
            }
            if (String.IsNullOrWhiteSpace(contextUser.Post))
            {
                error += "Введите должность\n";
            }
            if(IPhoto.Source == null)
            {
                error += "Загрузите фотографию\n";
            }
            contextUser.Gender = CBGender.SelectedItem as Gender;
            if(CBGender.SelectedItem == null)
            {
                error += "Выберите пол\n";
            }
            if (!String.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error);
                return;
            }
            App.DB.User.Add(contextUser);
            App.DB.SaveChanges();
        }

        private void BAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = ".png, .jpg, .jpeg | *.png; *.jpg; *.jpeg" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var image = File.ReadAllBytes(dialog.FileName);
                contextUser.Photo = image;
                DataContext = null;
                DataContext = contextUser;
            }
        }
    }
}
