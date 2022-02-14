using SongbookManagerLite.ViewModels;
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
    public partial class MusicPage : ContentPage
    {
        public MusicPage()
        {
            InitializeComponent();

            BindingContext = new MusicPageViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            var viewModel = (MusicPageViewModel)BindingContext;
            viewModel.UpdateMusicListCommand.Execute(null);
        }
    }
}