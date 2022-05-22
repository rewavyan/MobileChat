using MobileChat.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MobileChat.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}