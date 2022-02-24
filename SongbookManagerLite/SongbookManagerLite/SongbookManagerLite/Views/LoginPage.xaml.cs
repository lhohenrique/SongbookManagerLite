﻿using SongbookManagerLite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SongbookManagerLite.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = new LoginPageViewModel();
        }

        protected override async void OnAppearing()
        {
            var loggedUserEmail = Preferences.Get("Email", string.Empty);
            if (!String.IsNullOrEmpty(loggedUserEmail))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MusicPage());
            }
        }
    }
}