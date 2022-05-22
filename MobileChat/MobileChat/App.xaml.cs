using MobileChat.Services;
using MobileChat.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileChat
{
    public partial class App : Application
    {
        /*private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }*/

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            if (true)
            {
                Shell.Current.GoToAsync("//LoginPage");
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
