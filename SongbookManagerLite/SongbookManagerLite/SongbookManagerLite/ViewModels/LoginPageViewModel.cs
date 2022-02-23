using SongbookManagerLite.Models;
using SongbookManagerLite.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SongbookManagerLite.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region [Properties]
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

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmPassword"));
            }
        }

        private bool result;
        public bool Result
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return this.result; }
            set
            {
                this.result = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
            }
        }

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

        private bool isRegistering = false;
        public bool IsRegistering
        {
            get { return isRegistering; }
            set
            {
                isRegistering = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsRegistering"));
            }
        }

        private string nameErrorMessage = string.Empty;
        public string NameErrorMessage
        {
            get { return nameErrorMessage; }
            set
            {
                nameErrorMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NameErrorMessage"));
            }
        }

        private string emailErrorMessage = string.Empty;
        public string EmailErrorMessage
        {
            get { return emailErrorMessage; }
            set
            {
                emailErrorMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EmailErrorMessage"));            }
        }

        private string passwordErrorMessage = string.Empty;
        public string PasswordErrorMessage
        {
            get { return passwordErrorMessage; }
            set
            {
                passwordErrorMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PasswordErrorMessage"));
            }
        }
        #endregion

        #region [Commands]
        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }
        public Command SignUpCommand { get; set; }
        #endregion

        public LoginPageViewModel()
        {
            LoginCommand = new Command(async () => await LoginActionAsync());
            RegisterCommand = new Command(async () => await RegisterActionAsync());
            SignUpCommand = new Command(() => SignUpAction());
        }

        #region [Actions]
        private void SignUpAction()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;

            IsRegistering = true;
        }

        private async Task RegisterActionAsync()
        {
            bool infoValid = false;

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                infoValid = CheckInformations(Name, Email, Password, ConfirmPassword);

                if (infoValid)
                {
                    User newUser = new User()
                    {
                        Name = this.Name,
                        Email = this.Email,
                        Password = this.Password
                    };

                    await App.Database.RegisterUser(newUser);
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                if (infoValid)
                {
                    Name = string.Empty;
                    ConfirmPassword = string.Empty;
                    IsRegistering = false;
                }
                
                IsBusy = false;
            }
        }

        private async Task LoginActionAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                User userLogged = await App.Database.LoginUser(Email, Password);

                if(userLogged != null)
                {
                    Preferences.Set("Email", Email);
                    await Application.Current.MainPage.Navigation.PushAsync(new MusicPage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Usuário/Senha inválido(s)", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region [Private Methods]
        private bool CheckInformations(string nameToValidate, string emailToValidate, string passwordToValidate, string confirmPasswordToValidate)
        {
            var nameValid = ValidateName(nameToValidate);
            var emailValid = ValidateEmail(emailToValidate);
            var passwordValid = ValidatePassword(passwordToValidate, confirmPasswordToValidate);

            return nameValid && emailValid && passwordValid;
        }

        private bool ValidateName(string nameToValidate)
        {
            bool isValid = false;

            if (String.IsNullOrWhiteSpace(nameToValidate))
            {
                NameErrorMessage = "Informe um nome";
            }
            else
            {
                NameErrorMessage = string.Empty;
                isValid = true;
            }

            return isValid;
        }

        private bool ValidateEmail(string emailToValidate)
        {
            bool isValid = false;

            var emailPattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";

            if(String.IsNullOrWhiteSpace(emailToValidate) || !(Regex.IsMatch(emailToValidate, emailPattern)))
            {
                EmailErrorMessage = "Email inválido";
            }
            else
            {
                EmailErrorMessage = string.Empty;
                isValid = true;
            }

            return isValid;
        }

        private bool ValidatePassword(string passwordToValidade, string confirmPasswordToValidate)
        {
            bool isValid = false;

            if(!String.IsNullOrWhiteSpace(passwordToValidade) && passwordToValidade.Equals(confirmPasswordToValidate))
            {
                PasswordErrorMessage = string.Empty;
                isValid = true;
            }
            else
            {
                PasswordErrorMessage = "As senhas não correspondem";
            }

            return isValid;
        }
        #endregion
    }
}
