using Education.Models;
using Education.ViewProfile;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Education.ViewModels
{
    public class ProfileViewModel : ReactiveObject
    {
        [Reactive] public UserControl CurrentPage { get; set; }
        [Reactive] public Users UserShow { get; set; }
        [Reactive] public int CoursesCount { get; set; }

        public ReactiveCommand<Unit, Unit> ToOfflineCourses { get; set; }
        public ReactiveCommand<Unit, Unit> ToOnlineCourses { get; set; }

        private readonly educationEntities2 _entities;

        public ProfileViewModel()
        {
            _entities = new educationEntities2();
            CurrentPage = new OfflineCourses();

            ToOfflineCourses = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is OfflineCourses) return;
                CurrentPage = new OfflineCourses();
            });

            ToOnlineCourses = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is OnlineCourses) return;
                CurrentPage = new OnlineCourses();
            });

            LoadUserData();
            LoadFavoriteCourses();
        }

        private void LoadUserData()
        {
            var User = CurrentUser.NowUser;
            var GetContext = _entities.Users.FirstOrDefault(x => x.UserID == User.UserID);
            UserShow = GetContext;
        }

        private void LoadFavoriteCourses()
        {
            var User = CurrentUser.NowUser;
            var GetContext = _entities.Favorite.Where(x => x.UserID == User.UserID).ToList();
            CoursesCount = GetContext.Count;
        }
    }
}
