using MobileChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MobileChat.Services
{
    public class MockDataStore : IDataStore<Dialog>
    {
        User _user;
        Dialog _selectedDialog;
        readonly List<Dialog> dialogs;
        readonly List<LastMessage> lastMessages;

        public MockDataStore()
        {
            _user = new User();
            dialogs = new List<Dialog>();
        }

        public async Task<bool> SignIn(User user)
        {
            _user = user;
            return await Task.FromResult(true);
        }

        public async Task<User> GetUser()
        {
            return await Task.FromResult(_user);
        }

        public async Task<bool> SetSelectedDialog(Dialog dialog)
        {
            _selectedDialog = dialog;
            return await Task.FromResult(true);
        }

        public async Task<Dialog> GetSelectedDialog()
        {
            return await Task.FromResult(_selectedDialog);
        }

        public async Task<bool> AddDialogAsync(Dialog dialog)
        {
            dialogs.Add(dialog);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateDialogAsync(Dialog dialog)
        {
            var oldItem = dialogs.Where((Dialog arg) => arg.ID == dialog.ID).FirstOrDefault();
            dialogs.Remove(oldItem);
            dialogs.Add(dialog);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteDialogAsync(int id)
        {
            var oldItem = dialogs.Where((Dialog arg) => arg.ID == id).FirstOrDefault();
            dialogs.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Dialog> GetDialogAsync(int id)
        {
            return await Task.FromResult(dialogs.FirstOrDefault(s => s.ID == id));
        }

        public async Task<IEnumerable<Dialog>> GetDialogsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(dialogs);
        }

        public async Task<LastMessage> GetLastMessageAsync(Dialog dialog)
        {
            var webService = new WebService();
            return await webService.GetLastMessageAsync(dialog);
        }
    }
}