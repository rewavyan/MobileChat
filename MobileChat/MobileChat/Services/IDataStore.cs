using System;
using MobileChat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileChat.Services
{
    public interface IDataStore<Dialog>
    {
        Task<bool> SignIn(User user);
        Task<User> GetUser();
        Task<bool> SetSelectedDialog(Dialog dialog);
        Task<Dialog> GetSelectedDialog();
        Task<bool> AddDialogAsync(Dialog item);
        Task<bool> UpdateDialogAsync(Dialog item);
        Task<bool> DeleteDialogAsync(int id);
        Task<Dialog> GetDialogAsync(int id);
        Task<LastMessage> GetLastMessageAsync(Dialog dialog);
        Task<IEnumerable<Dialog>> GetDialogsAsync(bool forceRefresh = false);
    }
}
