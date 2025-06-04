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
    public partial class OfflineCourses : UserControl
    {
        private readonly educationEntities2 _entities;
        private offline_education[] OfflineShow;
        public OfflineCourses()
        {
            _entities = new educationEntities2();
            InitializeComponent();
            LoadFavoriteCourses();
            OneItem.ItemsSource = OfflineShow;
        }

        private void LoadFavoriteCourses()
        {
            var User = CurrentUser.NowUser;
            Favorite _context = new Favorite();
            var GetContext = _entities.Favorite.Where(x => x.UserID == User.UserID);
            var OffCourses = GetContext.Select(x => x.OffID);
            var OffShow = _entities.offline_education.Where(x => OffCourses.Contains(x.OffID)).ToArray();
            OfflineShow = OffShow;
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            var NowUser = CurrentUser.NowUser;
            if (sender is Button button && button.DataContext is offline_education offCourse)
            {
                var Favorite = _entities.Favorite.Where(x => x.OffID == offCourse.OffID && x.UserID == NowUser.UserID).FirstOrDefault();
                if (Favorite == null)
                {
                    return;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите отменить запись на курс «{offCourse.offName}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _entities.Favorite.Remove(Favorite);
                        _entities.SaveChanges();
                        MessageBox.Show($"Вы отменили запись на курс «{offCourse.offName}»");
                        LoadFavoriteCourses();
                        OneItem.ItemsSource = null;
                        OneItem.ItemsSource = OfflineShow;
                    }
                }
            }
        }
    }
}
