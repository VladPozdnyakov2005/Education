using Education.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Education.View
{
    public partial class ReportOffWindow : Window
    {
        private readonly educationEntities2 _entities;
        private int CourseID;
        private offline_education OfflineShow;
        private Users selectedUser;

        public ReportOffWindow(int courseID)
        {
            InitializeComponent();
            CourseID = courseID;
            _entities = new educationEntities2();
            OfflineShow = _entities.offline_education.FirstOrDefault(c => c.OffID == CourseID);

            LoadCourseData();
        }

        private void LoadCourseData()
        {
            if (OfflineShow != null)
            {
                CourseReportTextBlock.Text = $"Отчёт по курсу «{OfflineShow.offName}»";

                int totalSeats = (int)OfflineShow.seats;
                TotalSeatsTextBlock.Text = $"Общее количество мест – {totalSeats}";

                int filledSeats = _entities.Favorite.Count(f => f.OffID == CourseID);
                FilledSeatsTextBlock.Text = $"Количество заполненных мест – {filledSeats}";

                int remainingSeats = totalSeats - filledSeats;
                RemainingSeatsTextBlock.Text = $"Количество оставшихся мест – {remainingSeats}";

                var users = _entities.Favorite
                    .Where(f => f.OffID == CourseID)
                    .Select(f => f.Users)
                    .ToList();

                UsersDataGrid.ItemsSource = users;
                EditButton.DataContext = OfflineShow;
            }
            else
            {
                CourseReportTextBlock.Text = "Курс не найден";
            }
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null && UsersDataGrid.SelectedItem is Users)
            {
                DeleteButton.IsEnabled = true;
                selectedUser = (Users)UsersDataGrid.SelectedItem;
            }
            else
            {
                DeleteButton.IsEnabled = false;
                selectedUser = null;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя «{selectedUser.name} {selectedUser.surname}»?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var favoriteEntry = _entities.Favorite.FirstOrDefault(f => f.UserID == selectedUser.UserID && f.OffID == CourseID);
                    if (favoriteEntry != null)
                    {
                        _entities.Favorite.Remove(favoriteEntry);
                        _entities.SaveChanges();
                        var users = _entities.Favorite
                            .Where(f => f.OffID == CourseID)
                            .Select(f => f.Users)
                            .ToList();

                        UsersDataGrid.ItemsSource = users;
                        int totalSeats = (int)OfflineShow.seats;
                        int filledSeats = _entities.Favorite.Count(f => f.OffID == CourseID);
                        int remainingSeats = totalSeats - filledSeats;

                        TotalSeatsTextBlock.Text = $"Общее количество мест – {totalSeats}";
                        FilledSeatsTextBlock.Text = $"Количество заполненных мест – {filledSeats}";
                        RemainingSeatsTextBlock.Text = $"Количество оставшихся мест – {remainingSeats}";
                        this.UpdateLayout();
                        if (selectedUser != null)
                        {
                            MessageBox.Show($"Пользователь «{selectedUser.name} {selectedUser.surname}» успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                Console.WriteLine($"DataContext: {button.DataContext}");

                if (button.DataContext is offline_education OffEducation)
                {
                    EditOffWindow editOffWindow = new EditOffWindow(OffEducation.OffID, this);
                    editOffWindow.Show();
                    this.Hide();
                }
            }
        }
    }


}
