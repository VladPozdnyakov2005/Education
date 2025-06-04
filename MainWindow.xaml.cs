using Education.MainWindows;
using Education.Models;
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

namespace Education
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly educationEntities2 _Context;

        public MainWindow()
        {
            InitializeComponent();
            _Context = new educationEntities2();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(UsernameTextBox.Text))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            if (String.IsNullOrEmpty(DateTextBox.Text))
            {
                MessageBox.Show("Выберите дату вашего рождения");
                return;
            }

            string password = PasswordBox.Password;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            string confirmpassword = ConfirmPasswordBox.Password;
            if (string.IsNullOrWhiteSpace(confirmpassword))
            {
                MessageBox.Show("Подтвердите пароль");
                return;
            }

            if (password != confirmpassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            Users NewUser = new Users()
            {
                name = NameTextBox.Text,
                surname = SurnameTextBox.Text,
                login = UsernameTextBox.Text,
                birth = DateTextBox.SelectedDate,
                password = PasswordBox.Password,
                access = "USE"
            };
            _Context.Users.Add(NewUser);
            _Context.SaveChanges();
            MessageBox.Show("Регистрация прошла успешно");
            Login log = new Login();
            log.Show();
            this.Close();
        }

    }
}
