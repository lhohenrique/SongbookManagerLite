using SongbookManagerLite.Models;
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
    public partial class PreviewMusicPage : ContentPage
    {
        public PreviewMusicPage(Music music)
        {
            InitializeComponent();

            BindingContext = new PreviewMusicPageViewModel(Navigation, music);
        }

        protected override void OnAppearing()
        {
            var viewModel = (PreviewMusicPageViewModel)BindingContext;
            viewModel.ShowMusicDetailsCommand.Execute(null);
        }
    }
}