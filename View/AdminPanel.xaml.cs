using Education.MainWindows;
using Education.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Education.View
{
    public partial class AdminPanel : UserControl
    {
        public AdminPanel(Window mainWindow)
        {
            InitializeComponent();
            DataContext = new MainViewModel(mainWindow);
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.ToNull();
            Login loginWindow = new Login();
            loginWindow.Show();
            Window.GetWindow(this).Close();
        }
        private void AddOffButton_Click(object sender, RoutedEventArgs e)
        {
            AddOffWindow addOff = new AddOffWindow();
            addOff.Show();
        }

        private void AddOnButton_Click(object sender, RoutedEventArgs e)
        {
            AddOnWindow addOn = new AddOnWindow();
            addOn.Show();
        }
    }
}
