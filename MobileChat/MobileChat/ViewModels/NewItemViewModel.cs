using MobileChat.Models;
using MobileChat.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileChat.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string userIds;
        private string name;
        private string newMessage;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(userIds)
                && !String.IsNullOrWhiteSpace(newMessage);
        }

        public string UserIDs
        {
            get => userIds;
            set => SetProperty(ref userIds, value);
        }

        public string NewMessage
        {
            get => newMessage;
            set => SetProperty(ref newMessage, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var user = await DataStore.GetUser();
            var endChar = UserIDs.ToCharArray()[UserIDs.Length - 1] != ';' ? ";" : "";
            NewDialog newDialog = new NewDialog
            {
                UserID = user.ID,
                UserIDs = user.ID.ToString() + ";" + UserIDs + endChar,
                Name = Name,
                Type = string.IsNullOrWhiteSpace(Name) ? "private" : "public",
                Message = NewMessage
            };
            WebService webService = new WebService();
            var o = (NewDialog)await webService.GetResponse<NewDialog, NewDialog>(newDialog, Constants.AddDialogPage);
            if (o != null)
                await Shell.Current.GoToAsync("..");
        }

    }
    public class NewDialog
    {
        public int UserID { get; set; }
        public string UserIDs { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
