using Education.Models;
using System;
using System.Linq;
using System.Windows;

namespace Education.View
{
    public partial class EditOnWindow : Window
    {
        private readonly educationEntities2 _entities;
        private int CourseID;
        private online_education OnlineShow;
        private ReportOnWindow reportOnWindow;

        public EditOnWindow(int courseID, ReportOnWindow reportOnWindow = null)
        {
            InitializeComponent();
            _entities = new educationEntities2();
            CourseID = courseID;
            this.reportOnWindow = reportOnWindow;
            OnlineShow = _entities.online_education.FirstOrDefault(c => c.OnID == CourseID);

            if (OnlineShow != null)
            {
                CourseName.Text = OnlineShow.onName;
                CourseSrok.Text = OnlineShow.onSrok.ToString();
                CoursePlaces.Text = OnlineShow.onSeats.ToString();
                CoursePrice.Text = OnlineShow.onPrice.ToString();
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (OnlineShow != null)
            {
                OnlineShow.onName = CourseName.Text;
                OnlineShow.onSrok = int.Parse(CourseSrok.Text);
                OnlineShow.onSeats = int.Parse(CoursePlaces.Text);
                OnlineShow.onPrice = int.Parse(CoursePrice.Text);
                _entities.SaveChanges();
                MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (reportOnWindow != null)
            {
                reportOnWindow.Show();
            }
            this.Close();
        }
    }
}
