using MobileChat.Models;
using MobileChat.Services;
using MobileChat.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MobileChat.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {

        private string name;
        private string login;
        private string password;
        private string error;

        public Command BackCommand { get; }
        public Command RegistrationCommand { get; }

        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Login { get => login; set => SetProperty(ref login, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string Error { get => error; set => SetProperty(ref error, value); }

        public RegistrationViewModel()
        {
            BackCommand = new Command(OnBackClick);
            RegistrationCommand = new Command(OnRegistrationClick);
        }

        private async void OnBackClick(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void OnRegistrationClick(object obj)
        {
            User user = new User
            {
                Name = Name,
                Login = Login,
                Password = Password
            };
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            string json = JsonConvert.SerializeObject(user);
            request.RequestUri = new Uri(Constants.RegistrationPage);
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(json);
            request.Headers.Add("Accept", "application/json");
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var responsejson = await responseContent.ReadAsStringAsync(Encoding.UTF8);
                    try
                    {
                        var data = JsonConvert.DeserializeObject<User>(responsejson);
                        if (data.ID.ToString() == "" || data.ID.ToString() == "0")
                            Error = "Ошибка регистрации!";
                        else
                        {
                            Error = "Регистрация прошла успешно!\nВернитесь на страницу входа";
                            //await DataStore.SignIn(user);
                            //await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Error = "Ошибка регистрации!";
                    }
                }
            }
            catch
            {
                Error = "Не удалось подключиться к серверу!";
            }
        }

    }
}
