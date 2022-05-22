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
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegistrationCommand { get; }

        private string _login;
        private string _password;
        private string _error;
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string Error
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegistrationCommand = new Command(OnRegistrationClick);
        }

        private async void OnRegistrationClick(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}");
        }

        private async void OnLoginClicked(object obj)
        {
            User user = new User
            {
                Login = Login,
                Password = Password
            };

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            string json = JsonConvert.SerializeObject(user);
            request.RequestUri = new Uri(Constants.SignInPage);
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
                        //l_state.Text = decodejson.ToString();
                        if (data.Name is null)
                            Error = "Неверный логин или пароль!";
                        else
                        {
                            await DataStore.SignIn(data);
                            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Error = "Неверный логин или пароль!\n" + ex.ToString();
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
