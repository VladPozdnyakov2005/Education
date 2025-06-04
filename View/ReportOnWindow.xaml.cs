using Education.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Education.View
{
    public partial class ReportOnWindow : Window
    {
        private readonly educationEntities2 _entities;
        private int CourseID;
        private online_education OnlineShow;
        private Users selectedUser;

        public ReportOnWindow(int courseID)
        {
            InitializeComponent();
            CourseID = courseID;
            _entities = new educationEntities2();
            OnlineShow = _entities.online_education.FirstOrDefault(c => c.OnID == CourseID);

            if (OnlineShow != null)
            {
                CourseReportTextBlock.Text = $"Отчёт по курсу «{OnlineShow.onName}»";
                int totalSeats = (int)OnlineShow.onSeats;
                TotalSeatsTextBlock.Text = $"Общее количество мест – {totalSeats}";
                int filledSeats = _entities.Favorite.Count(f => f.OnID == CourseID);
                FilledSeatsTextBlock.Text = $"Количество заполненных мест – {filledSeats}";
                int remainingSeats = totalSeats - filledSeats;
                RemainingSeatsTextBlock.Text = $"Количество оставшихся мест – {remainingSeats}";
                int coursePrice = (int)OnlineShow.onPrice;
                CoursePriceTextBlock.Text = $"Цена курса – {coursePrice}";
                var users = _entities.Favorite
                    .Where(f => f.OnID == CourseID)
                    .Select(f => f.Users)
                    .ToList();

                UsersDataGrid.ItemsSource = users;
                EditButton.DataContext = OnlineShow;
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
                    var favoriteEntry = _entities.Favorite.FirstOrDefault(f => f.UserID == selectedUser.UserID && f.OnID == CourseID);
                    if (favoriteEntry != null)
                    {
                        _entities.Favorite.Remove(favoriteEntry);
                        _entities.SaveChanges();
                        var users = _entities.Favorite
                            .Where(f => f.OnID == CourseID)
                            .Select(f => f.Users)
                            .ToList();

                        UsersDataGrid.ItemsSource = users;
                        int totalSeats = (int)OnlineShow.onSeats;
                        int filledSeats = _entities.Favorite.Count(f => f.OnID == CourseID);
                        int remainingSeats = totalSeats - filledSeats;
                        int coursePrice = (int)OnlineShow.onPrice;

                        TotalSeatsTextBlock.Text = $"Общее количество мест – {totalSeats}";
                        FilledSeatsTextBlock.Text = $"Количество заполненных мест – {filledSeats}";
                        RemainingSeatsTextBlock.Text = $"Количество оставшихся мест – {remainingSeats}";
                        CoursePriceTextBlock.Text = $"Цена курса – {coursePrice} руб.";
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

                if (button.DataContext is online_education OnEducation)
                {
                    EditOnWindow editOnWindow = new EditOnWindow(OnEducation.OnID, this);
                    editOnWindow.Show();
                    this.Hide();
                }
            }
        }
    }
}
