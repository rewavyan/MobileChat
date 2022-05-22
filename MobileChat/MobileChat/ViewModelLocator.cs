/*using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using MobileChat.ViewModels;

namespace MobileChat
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ItemsViewModel>();
        }

        public LoginViewModel LoginVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
        public ItemsViewModel ItemsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemsViewModel>();
            }
        }
    }
}*/
