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

namespace Education.ViewProfile
{
    public partial class OnlineCourses : UserControl
    {
        private readonly educationEntities2 _entities;
        private online_education[] OnlineShow;
        public OnlineCourses()
        {
            _entities = new educationEntities2();
            InitializeComponent();
            LoadFavoriteCourses();
            OneItem.ItemsSource = OnlineShow;
        }

        private void LoadFavoriteCourses()
        {
            var User = CurrentUser.NowUser;
            Favorite _context = new Favorite();
            var GetContext = _entities.Favorite.Where(x => x.UserID == User.UserID);
            var OnCourses = GetContext.Select(x => x.OnID);
            var OnShow = _entities.online_education.Where(x => OnCourses.Contains(x.OnID)).ToArray();
            OnlineShow = OnShow;

        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            var NowUser = CurrentUser.NowUser;
            if (sender is Button button && button.DataContext is online_education onCourse)
            {
                var Favorite = _entities.Favorite.Where(x => x.OnID == onCourse.OnID && x.UserID == NowUser.UserID).FirstOrDefault();
                if (Favorite == null)
                {
                    return;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите отменить запись на курс «{onCourse.onName}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _entities.Favorite.Remove(Favorite);
                        _entities.SaveChanges();
                        MessageBox.Show($"Вы отменили запись на курс «{onCourse.onName}»");

                        LoadFavoriteCourses();
                        OneItem.ItemsSource = null;
                        OneItem.ItemsSource = OnlineShow;
                    }
                }
            }
        }

    }
}
