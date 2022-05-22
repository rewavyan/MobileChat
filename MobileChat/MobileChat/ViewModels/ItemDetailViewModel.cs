using MobileChat.Models;
using MobileChat.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileChat.ViewModels
{
    [QueryProperty(nameof(DialogID), nameof(DialogID))]
    public class ItemDetailViewModel : BaseViewModel
    {
        WebService webService;
        User user;
        private int dialogID;
        private Dialog Dialog;
        private ObservableCollection<Message> _messages;
        private string _newMessage;

        public Command SendMessageCommand { get; }

        public ItemDetailViewModel()
        {
            webService = new WebService();
            SendMessageCommand = new Command(OnSendMessage);
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                Task.Factory.StartNew(async() => 
                {
                    var count = _messages.Count;
                    var messages = (List<Message>)await webService.GetResponse<MessageRequest, List<Message>>(new MessageRequest { DialogID = DialogID }, Constants.GetMessagesPage);
                    if (count != messages.Count)
                        LoadMessages();
                });
                return true;
            });
        }

        private async void OnSendMessage()
        {
            var message = new Message
            {
                DialogID = dialogID.ToString(),
                UserID = user.ID.ToString(),
                Value = NewMessage,
                DateTimeSent = DateTime.Now
            };
            var o = (Message)await webService.GetResponse<Message, Message>(message, Constants.SendMessagePage);
            if (o != null)
            {
                NewMessage = string.Empty;
                LoadMessages();
            }
        }
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }
        public string NewMessage
        {
            get => _newMessage;
            set => SetProperty(ref _newMessage, value);
        }

        public int DialogID
        {
            get
            {
                return dialogID;
            }
            set
            {
                dialogID = value;
                LoadDialog();
                LoadMessages();
            }
        }

        public async void LoadDialog()
        {
            try
            {
                user = await DataStore.GetUser();
                Dialog = await DataStore.GetSelectedDialog();
                Title = Dialog.Name;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public async void LoadMessages()
        {
            var messages = (List<Message>)await webService.GetResponse<MessageRequest, List<Message>>(new MessageRequest { DialogID = DialogID }, Constants.GetMessagesPage);
            Messages = new ObservableCollection<Message>();
            foreach (var message in messages)
            {
                if (message.UserID == user.ID.ToString())
                {
                    message.UserName = "Вы";
                    message.IsYour = true;
                    message.BoxColor = Color.FromRgb(213, 226, 242);
                    message.BoxMargin = new Thickness { Left = 32 };
                    message.MessageMargin = new Thickness { Left = 40, Right = 6, Bottom = 4, Top = 4 };
                    message.DateTimeMargin = new Thickness { Right = 2, Bottom = 2 };
                }
                else
                {
                    var appUser = (User)await webService.GetResponse<User, User>(new User { ID = Convert.ToInt32(message.UserID) }, Constants.GetUserPage);
                    message.UserName = "От " + appUser.Name;
                    message.IsYour = false;
                    message.BoxColor = Color.FromRgb(232, 233, 235);
                    message.BoxMargin = new Thickness { Right = 32 };
                    message.MessageMargin = new Thickness { Left = 4, Right = 36, Bottom = 4, Top = 4 };
                    message.DateTimeMargin = new Thickness { Right = 36, Bottom = 2 };
                }
                Messages.Add(message);
            }
        }
    }

    public class MessageRequest
    {
        public int DialogID { get; set; }
    }
}
