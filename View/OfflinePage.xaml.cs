using Education.MainWindows;
using Education.Models;
using Education.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Objects.DataClasses;
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
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Education.View
{

    public partial class OfflinePage : UserControl
    {
        private readonly educationEntities2 _entities;

        public OfflinePage()
        {
            InitializeComponent();
            _entities = new educationEntities2();
            OfflineShow = _entities.offline_education.ToArray();
            OneItem.ItemsSource = OfflineShow;
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
                            var course = item as offline_education;
                            var NowUser = CurrentUser.NowUser;
                            if (course != null && NowUser != null)
                            {
                                var CurrentFavorite = _entities.Favorite.FirstOrDefault(x => x.UserID == NowUser.UserID && x.OffID == course.OffID);
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

        public offline_education[] OfflineShow { get; }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.IsLogin == true)
            {
                if (sender is Button button && button.DataContext is offline_education OffEducation)
                {
                    if (CurrentUser.IsAdmin())
                    {
                        ReportOffWindow reportWindow = new ReportOffWindow(OffEducation.OffID);
                        reportWindow.Show();
                    }
                    else
                    {
                        var NowUser = CurrentUser.NowUser;
                        if (NowUser != null)
                        {
                            var CurrentFavorite = _entities.Favorite.FirstOrDefault(x => x.UserID == NowUser.UserID && x.OffID == OffEducation.OffID);
                            if (CurrentFavorite != null)
                            {
                                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите отменить запись на курс «{OffEducation.offName}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    button.Background = Brushes.LightYellow;
                                    _entities.Favorite.Remove(CurrentFavorite);
                                    _entities.SaveChanges();
                                    MessageBox.Show($"Вы отменили запись на курс «{OffEducation.offName}»");
                                }
                            }
                            else
                            {
                                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите записаться на курс «{OffEducation.offName}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (result == MessageBoxResult.Yes)
                                {
                                    Favorite Favor = new Favorite()
                                    {
                                        UserID = NowUser.UserID,
                                        OffID = OffEducation.OffID,
                                    };
                                    button.Background = Brushes.MediumSeaGreen;
                                    _entities.Favorite.Add(Favor);
                                    _entities.SaveChanges();
                                    MessageBox.Show($"Вы записались на курс «{OffEducation.offName}»");
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