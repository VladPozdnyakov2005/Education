using Education.View;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;

namespace Education.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private Window _mainWindow;

        [Reactive] public UserControl CurrentPage { get; set; }
        public ReactiveCommand<Unit, Unit> ToHomePage { get; set; }
        public ReactiveCommand<Unit, Unit> ToOfflinePage { get; set; }
        public ReactiveCommand<Unit, Unit> ToInformationPage { get; set; }
        public ReactiveCommand<Unit, Unit> ToFeedbackPage { get; set; }
        public ReactiveCommand<Unit, Unit> ToOnlinePage { get; set; }
        public ReactiveCommand<Unit, Unit> ToAdminPage { get; set; }

        public MainViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
            CurrentPage = new HomePage();

            ToHomePage = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is HomePage) return;
                CurrentPage = new HomePage();
            });

            ToOfflinePage = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is OfflinePage) return;
                CurrentPage = new OfflinePage();
            });

            ToOnlinePage = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is OnlinePage) return;
                CurrentPage = new OnlinePage();
            });

            ToInformationPage = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is InformationPage) return;
                CurrentPage = new InformationPage();
            });

            ToFeedbackPage = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is FeedbackPage) return;
                CurrentPage = new FeedbackPage();
            });

            ToAdminPage = ReactiveCommand.Create(() =>
            {
                if (CurrentPage is AdminPanel) return;
                CurrentPage = new AdminPanel(_mainWindow);
            });
        }
    }
}
