using Education.Models;
using System;
using System.Windows;

namespace Education.View
{
    public partial class AddOnWindow : Window
    {
        private readonly educationEntities2 _entities;

        public AddOnWindow()
        {
            InitializeComponent();
            _entities = new educationEntities2();
        }

        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CourseName.Text))
            {
                MessageBox.Show("Пожалуйста, введите название курса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(CourseSrok.Text) || !int.TryParse(CourseSrok.Text, out int srok) || srok <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для срока курса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(CoursePlaces.Text) || !int.TryParse(CoursePlaces.Text, out int places) || places <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для количества мест", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(CoursePrice.Text) || !int.TryParse(CoursePrice.Text, out int price) || price <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для цены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var newCourse = new online_education
                {
                    onName = CourseName.Text,
                    onSrok = srok,
                    onSeats = places,
                    onPrice = price,
                    courseType = "Online"
                };

                _entities.online_education.Add(newCourse);
                _entities.SaveChanges();

                MessageBox.Show("Курс успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка введенных данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
