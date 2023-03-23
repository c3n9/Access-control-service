using AccessControlService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace AccessControlService.Pages
{
    /// <summary>
    /// Логика взаимодействия для PRecovery.xaml
    /// </summary>
    public partial class PRecovery : Page
    {
        User contextUser;
        public PRecovery(User user)
        {
            InitializeComponent();
            App.MainWindowInstance.BBack.Visibility = Visibility.Visible;
            contextUser = user;
            DataContext= contextUser;
        }
    }
}
