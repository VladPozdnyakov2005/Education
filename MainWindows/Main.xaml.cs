using System;
using Education.ViewModels;
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
using System.Windows.Shapes;

namespace Education.MainWindows
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
            UserAdminOrNull();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

        private void UserAdminOrNull()
        {
            if (CurrentUser.IsLogin)
            {
                enter.Visibility = Visibility.Hidden;

                if (CurrentUser.IsAdmin())
                {
                    AdminButton.Visibility = Visibility.Visible;
                    profileButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    profileButton.Visibility = Visibility.Visible;
                    AdminButton.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                enter.Visibility = Visibility.Visible;
                profileButton.Visibility = Visibility.Hidden;
                AdminButton.Visibility = Visibility.Hidden;
            }
        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            Profile ProfileWindow = new Profile();
            ProfileWindow.Show();
            this.Close();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.ToNull();
            Login LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }
    }
}
