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
        int index = 0;
        User contextUser = new User();
        User employee;
        public PAccessControl(User user)
        {
            InitializeComponent();
            employee= user;
            BanTime();
            TBFullName.Text = $"{user.Surname} {user.Name[0]}. {user.Patronymic[0]}.";
            App.MainWindowInstance.BBack.Visibility = Visibility.Visible;
            DataContext = contextUser;
            CBGender.ItemsSource = App.DB.Gender.ToList();
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            index++;
            BanTime();
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

            contextUser = new User();
            DataContext = null;
            DataContext = contextUser;
            MessageBox.Show("Пользователь сохранен");
        }

        private void BanTime()
        {
            if(employee.BanTime == null && index == 2)
            {
                employee.BanTime = DateTime.Now;
                App.DB.SaveChanges();
            }
            if (employee.BanTime != null)
            {
                if (index == 2 || employee.BanTime.Value.Minute + 5 > DateTime.Now.Minute)//если не закрывать приложение снятие бана по истечению времени работает
                {
                    employee.BanTime = DateTime.Now;
                    App.DB.SaveChanges();
                    index = 0;
                    TBName.IsEnabled = false;
                    TBSurname.IsEnabled = false;
                    TBPatronymic.IsEnabled = false;
                    BCancel.IsEnabled = false;
                    BSave.IsEnabled = false;
                    TBPost.IsEnabled = false;
                    HLAddPhoto.IsEnabled = false;
                    CBGender.IsEnabled = false;
                }
                else
                {
                    TBName.IsEnabled = true;
                    TBSurname.IsEnabled = true;
                    TBPatronymic.IsEnabled = true;
                    BCancel.IsEnabled = true;
                    BSave.IsEnabled = true;
                    TBPost.IsEnabled = true;
                    HLAddPhoto.IsEnabled = true;
                    CBGender.IsEnabled = true;
                }
            }
            else
            {
                TBName.IsEnabled = true;
                TBSurname.IsEnabled = true;
                TBPatronymic.IsEnabled = true;
                BCancel.IsEnabled = true;
                BSave.IsEnabled = true;
                TBPost.IsEnabled = true;
                HLAddPhoto.IsEnabled = true;
                CBGender.IsEnabled = true;
            }
        }
        private void HLAddPhoto_Click(object sender, RoutedEventArgs e)
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

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            contextUser = new User();
            DataContext = null;
            DataContext = contextUser;
            MessageBox.Show("Данные очищены");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BanTime();
        }
    }
}
