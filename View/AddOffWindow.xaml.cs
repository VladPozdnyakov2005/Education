using Education.Models;
using System;
using System.Windows;

namespace Education.View
{
    public partial class AddOffWindow : Window
    {
        private readonly educationEntities2 _entities;

        public AddOffWindow()
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
                MessageBox.Show("Пожалуйста, введите корректное значение для срока курса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(CoursePlaces.Text) || !int.TryParse(CoursePlaces.Text, out int places) || places <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для количества мест.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите структуру.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var newCourse = new offline_education
                {
                    offName = CourseName.Text,
                    offSrok = srok,
                    seats = places,
                    divisNum = ComboBox.SelectedIndex + 1,
                    courseType = "Offline"
                };

                _entities.offline_education.Add(newCourse);
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
