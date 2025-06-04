using Education.MainWindows;
using Education.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Education.View
{
    public partial class OnlinePage : UserControl
    {
        private readonly educationEntities2 _entities;

        public OnlinePage()
        {
            InitializeComponent();
            _entities = new educationEntities2();
            OnlineShow = _entities.online_education.ToArray();
            OneItem.ItemsSource = OnlineShow;
            this.Loaded += (sender, e) => UserAdminOrNull();
        }

        private void UserAdminOrNull()
        {
            if (CurrentUser.IsAdmin())
            {
                foreach (var item in OneItem.Items)
                {
                    var container = OneItem.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                    if (container != null)
                    {
                        var button = FindVisualChild<Button>(container, "FavoriteButton");
                        if (button != null)
                        {
                            button.Content = "Отчёт по курсу";
                        }
                    }
                    else
                    {
                        OneItem.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
                    }
                }
            }
            else
            {
                foreach (var item in OneItem.Items)
                {
                    var container = OneItem.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                    if (container != null)
                    {
                        var button = FindVisualChild<Button>(container, "FavoriteButton");
                        if (button != null)
                        {
                            button.Content = "Записаться на курс";
                            var course = item as online_education;
                            var NowUser = CurrentUser.NowUser;
                            if (course != null && NowUser != null)
                            {
                                var CurrentFavorite = _entities.Favorite.FirstOrDefault(x => x.UserID == NowUser.UserID && x.OnID == course.OnID);
                                if (CurrentFavorite != null)
                                {
                                    button.Background = Brushes.MediumSeaGreen;
                                }
                                else
                                {
                                    button.Background = Brushes.LightYellow;
                                }
                            }
                        }
                    }
                    else
                    {
                        OneItem.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
                    }
                }
            }
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (OneItem.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                UserAdminOrNull();
                OneItem.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
            }
        }

        private static T FindVisualChild<T>(DependencyObject obj, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T typedChild && (childName == null || ((FrameworkElement)child).Name == childName))
                {
                    return typedChild;
                }

                var childOfChild = FindVisualChild<T>(child, childName);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        public online_education[] OnlineShow { get; }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.IsLogin == true)
            {
                if (sender is Button button && button.DataContext is online_education OnEducation)
                {
                    if (CurrentUser.IsAdmin())
                    {
                        var reportWindow = new ReportOnWindow(OnEducation.OnID);
                        reportWindow.Show();
                    }
                    else
                    {
                        var NowUser = CurrentUser.NowUser;
                        if (NowUser != null)
                        {
                            var CurrentFavorite = _entities.Favorite.FirstOrDefault(x => x.UserID == NowUser.UserID && x.OnID == OnEducation.OnID);
                            if (CurrentFavorite != null)
                            {
                                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите отменить запись на курс «{OnEducation.onName}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    button.Background = Brushes.LightYellow;
                                    _entities.Favorite.Remove(CurrentFavorite);
                                    _entities.SaveChanges();
                                    MessageBox.Show($"Вы отменили запись на курс «{OnEducation.onName}»");
                                }
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите записаться на курс «{OnEducation.onName}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    Favorite Favor = new Favorite()
                                    {
                                        UserID = NowUser.UserID,
                                        OnID = OnEducation.OnID,
                                    };
                                    button.Background = Brushes.MediumSeaGreen;
                                    _entities.Favorite.Add(Favor);
                                    _entities.SaveChanges();
                                    MessageBox.Show($"Вы записались на курс «{OnEducation.onName}»");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Чтобы записаться на курс, нужно войти в аккаунт");
                return;
            }
        }
    }


}
