using MobileChat.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileChat.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private User user;
        public User User
        {
            get => user;
            set => SetProperty(ref user, value);
        }
        public AboutViewModel()
        {
            Title = "Профиль";
        }

        public void OnAppearing()
        {
            IsBusy = true;
            User = Task.Run(async () => await DataStore.GetUser()).Result;
        }
    }
}