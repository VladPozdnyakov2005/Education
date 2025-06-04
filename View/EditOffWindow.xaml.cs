using Education.Models;
using System;
using System.Linq;
using System.Windows;

namespace Education.View
{
    public partial class EditOffWindow : Window
    {
        private readonly educationEntities2 _entities;
        private int CourseID;
        private offline_education OfflineShow;
        private ReportOffWindow reportOffWindow;

        public EditOffWindow(int courseID, ReportOffWindow reportOffWindow = null)
        {
            InitializeComponent();
            _entities = new educationEntities2();
            CourseID = courseID;
            this.reportOffWindow = reportOffWindow;
            OfflineShow = _entities.offline_education.FirstOrDefault(c => c.OffID == CourseID);

            if (OfflineShow != null)
            {
                CourseName.Text = OfflineShow.offName;
                CourseSrok.Text = OfflineShow.offSrok.ToString();
                CoursePlaces.Text = OfflineShow.seats.ToString();
                switch (OfflineShow.divisNum)
                {
                    case 1:
                        ComboBox.SelectedIndex = 0;
                        break;
                    case 2:
                        ComboBox.SelectedIndex = 1;
                        break;
                    case 3:
                        ComboBox.SelectedIndex = 2;
                        break;
                    case 4:
                        ComboBox.SelectedIndex = 3;
                        break;
                    default:
                        ComboBox.SelectedIndex = -1;
                        break;
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (OfflineShow != null)
            {
                OfflineShow.offName = CourseName.Text;
                OfflineShow.offSrok = int.Parse(CourseSrok.Text);
                OfflineShow.seats = int.Parse(CoursePlaces.Text);
                if (ComboBox.SelectedIndex != -1)
                {
                    OfflineShow.divisNum = ComboBox.SelectedIndex + 1;
                }
                _entities.SaveChanges();

                MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (reportOffWindow != null)
            {
                reportOffWindow.Show();
            }
            this.Close();
        }
    }
}
