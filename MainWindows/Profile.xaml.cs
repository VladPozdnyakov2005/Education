using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Education.Models;
using DynamicData;
using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using Education.ViewModels;
using Education.ViewProfile;

namespace Education.MainWindows
{
    public partial class Profile : Window
    {
        private static string imagePath;
        private readonly educationEntities2 _entities;
        private offline_education[] OfflineShow;
        private online_education[] OnlineShow;
        public Users UserShow { get; set; }
        private ProfileViewModel _viewModel;

        public Profile()
        {
            InitializeComponent();
            _entities = new educationEntities2();
            _viewModel = new ProfileViewModel();
            LoadDefaultImage();
            LoadImage();
            DataContext = _viewModel;
        }

        private void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    imagePath = openFileDialog.FileName;
                    BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                    ProfileImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки картинки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadDefaultImage()
        {
            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri("pack://application:,,,/assets/Guest2.png"));
                ProfileImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки картинки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadImage()
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                    ProfileImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки картинки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.ToNull();
            Main Home = new Main();
            Home.Show();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Main Home = new Main();
            Home.Show();
            this.Close();
        }

    }
}
