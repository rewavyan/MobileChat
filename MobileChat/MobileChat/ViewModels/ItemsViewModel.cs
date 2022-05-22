using MobileChat.Models;
using MobileChat.Services;
using MobileChat.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileChat.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private DialogWithLastMessage _selectedItem;
        private ObservableCollection<DialogWithLastMessage> _dialogs;
        private User User;
        public ObservableCollection<DialogWithLastMessage> Dialogs
        {
            get => _dialogs;
            set => SetProperty(ref _dialogs, value);
        }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<DialogWithLastMessage> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Диалоги";
            
            Dialogs = new ObservableCollection<DialogWithLastMessage>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<DialogWithLastMessage>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Task.Factory.StartNew(async () =>
                {
                    await GetDialogs();
                });
                return true;
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            User = await DataStore.GetUser();
            //Title = User.Login;
            IsBusy = true;
            try
            {
                var dialogs = await DataStore.GetDialogsAsync(true);
                var dialogsWlm = new ObservableCollection<DialogWithLastMessage>();
                foreach (var dialog in dialogs)
                {
                    var lastMessage = await DataStore.GetLastMessageAsync(dialog);
                    dialogsWlm.Add(new DialogWithLastMessage { Dialog = dialog, LastMessage = lastMessage});
                }
                Dialogs.Clear();
                Dialogs = dialogsWlm;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
            Task.Factory.StartNew(async () =>
            {
                await GetDialogs();
            });
        }

        public DialogWithLastMessage SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(DialogWithLastMessage dialog)
        {
            if (dialog == null)
                return;
            await DataStore.SetSelectedDialog(dialog.Dialog);
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.DialogID)}={dialog.Dialog.ID}");
        }

        private async Task<bool> GetDialogs()
        {
            var user = await DataStore.GetUser();
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            string json = JsonConvert.SerializeObject(user);
            request.RequestUri = new Uri(Constants.GetDialogsPage);
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
                        var data = JsonConvert.DeserializeObject<List<Dialog>>(responsejson);
                        var dialogs = new ObservableCollection<DialogWithLastMessage>();
                        foreach (var dialog in data)
                        {
                            if (string.IsNullOrEmpty(dialog.Name))
                            {
                                var userIds = dialog.UserIDs.Split(';');
                                var userId = Convert.ToInt32(userIds.Where(u => Convert.ToInt32(u) != user.ID).FirstOrDefault());
                                HttpRequestMessage request1 = new HttpRequestMessage();
                                string json1 = JsonConvert.SerializeObject(new { ID = userId });
                                request1.RequestUri = new Uri(Constants.GetUserPage);
                                request1.Method = HttpMethod.Post;
                                request1.Content = new StringContent(json1);
                                request1.Headers.Add("Accept", "application/json");
                                try
                                {
                                    HttpResponseMessage response1 = await client.SendAsync(request1);
                                    if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        HttpContent responseContent1 = response1.Content;
                                        var responsejson1 = await responseContent1.ReadAsStringAsync(Encoding.UTF8);
                                        try
                                        {
                                            var data1 = JsonConvert.DeserializeObject<User>(responsejson1);
                                            dialog.Name = data1.Name;
                                            var lastMessage = await DataStore.GetLastMessageAsync(dialog);
                                            dialogs.Add(new DialogWithLastMessage { Dialog = dialog, LastMessage = lastMessage });
                                            
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            {
                                var lastMessage = await DataStore.GetLastMessageAsync(dialog);
                                dialogs.Add(new DialogWithLastMessage { Dialog = dialog, LastMessage = lastMessage });
                            }

                        }
                        Dialogs = dialogs;
                        //dialogs.AddRange(data);
                    }
                    catch (Exception ex)
                    {
                        //Error = "Неверный логин или пароль!\n" + ex.ToString();
                    }
                }
            }
            catch
            {
                //Error = "Не удалось подключиться к серверу!";
            }
            return true;
        }
    }
}