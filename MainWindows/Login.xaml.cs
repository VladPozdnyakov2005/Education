using Education.Models;
using Education.View;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Education.MainWindows
{
    public partial class Login : Window
    {
        private readonly educationEntities2 _context;
        public Login()
        {
            InitializeComponent();
            _context = new educationEntities2();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
           MainWindow Registration = new MainWindow();
            Registration.Show();
            this.Close();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Main Home = new Main();
            Home.Show();
            this.Close();
        }
        private void Login_Button(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                MessageBox.Show("Введите логин");
                return;
            }
            string password = PasswordTextBox.Password;
            if (string.IsNullOrEmpty( password) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            
            var User = _context.Users.Where(u=> u.login == LoginTextBox.Text && u.password == password).FirstOrDefault();
            if (User == null)
            {
                MessageBox.Show("Неправильно введён логин или пароль");
                return;
            }
            {
                MessageBox.Show("Вы вошли в аккаунт");
                CurrentUser.SetUser(User);
            }
            Main Home = new Main();
            Home.Show();
            this.Close();
        }

        
        
    }
}
