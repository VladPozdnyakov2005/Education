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

namespace Education.View
{
    public partial class FeedbackPage : UserControl
    {
        private readonly educationEntities2 _entities;
        public FeedbackPage()
        {
            InitializeComponent();
            _entities = new educationEntities2();
            FeedbackShow = _entities.Clients.ToArray();
            OneItem.ItemsSource = FeedbackShow;
        }
        public Clients[] FeedbackShow { get; }
    }
    
}
