﻿using SongbookManagerLite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SongbookManagerLite.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SharePage : ContentPage
    {
        public SharePage()
        {
            InitializeComponent();

            BindingContext = new SharePageViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            var viewModel = (SharePageViewModel)BindingContext;
            viewModel.UpdateUserListCommand.Execute(null);
            viewModel.HandleShareButton();
        }
    }
}