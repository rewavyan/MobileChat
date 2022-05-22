using MobileChat.Models;
using MobileChat.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileChat.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Dialog Dialog { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}