using SongbookManagerLite.Helpers;
using SongbookManagerLite.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SongbookManagerLite.ViewModels
{
    public class ProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }

        #region [Properties]
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        #endregion

        #region [Commands]
        public Command ChangePasswordCommand { get; set; }
        public Command FeedbackCommand { get; set; }
        public Command RateAppCommand { get; set; }
        public Command SettingsCommand { get; set; }
        public Command PrivacyPolicyCommand { get; set; }
        public Command LogoutCommand { get; set; }
        #endregion

        public ProfilePageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            ChangePasswordCommand = new Command(() => ChangePasswordAction());
            FeedbackCommand = new Command(() => FeedbackAction());
            RateAppCommand = new Command(() => RateAppAction());
            SettingsCommand = new Command(() => SettingsAction());
            PrivacyPolicyCommand = new Command(() => PrivacyPolicyAction());
            LogoutCommand = new Command(() => LogoutAction());
        }

        #region [Actions]
        public void ChangePasswordAction()
        {
            Navigation.PushAsync(new ChangePasswordPage());
        }

        public void FeedbackAction()
        {
            // Open email to send feedback
        }

        public void RateAppAction()
        {
            // Open store
        }

        public void SettingsAction()
        {
            // Navigate to settings page
        }

        public void PrivacyPolicyAction()
        {
            // Navigate to privacy policy page
        }

        public async void LogoutAction()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Tem certeza de que deseja sair?", string.Empty, "Sair", "Cancelar");

            if (result)
            {
                Preferences.Clear();
                LoggedUserHelper.ResetLoggedUser();

                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
        #endregion

        #region [Public Methods]
        public void OnAppearing()
        {
            Name = LoggedUserHelper.LoggedUser.Name;
            Email = LoggedUserHelper.LoggedUser.Email;
        }
        #endregion
    }
}
