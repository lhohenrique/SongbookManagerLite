﻿using SongbookManagerLite.Helpers;
using SongbookManagerLite.Models;
using SongbookManagerLite.Services;
using SongbookManagerLite.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SongbookManagerLite.ViewModels
{
    public class ForgotPasswordPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        SmtpClient smtpServer;

        private UserService userService;
        private string newPassword;
        private User user;

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
        #endregion

        #region [Commands]
        public Command ResetPasswordCommand { get; set; }
        #endregion

        public ForgotPasswordPageViewModel()
        {
            userService = new UserService();

            ResetPasswordCommand = new Command(async () => await ResetPasswordActionAsync());
        }

        #region [Action]
        public async Task ResetPasswordActionAsync()
        {
            try
            {
                user = await userService.GetUser(Email);

                if (user != null)
                {
                    // Create a random password with 6 characters
                    Random rd = new Random();
                    int randomNumber = rd.Next(100000, 999999);
                    newPassword = randomNumber.ToString();

                    // Enviar email com nova senha
                    smtpServer = new SmtpClient("smtp.gmail.com");
                    smtpServer.Port = 587;
                    smtpServer.Host = "smtp.gmail.com";
                    smtpServer.EnableSsl = true;
                    smtpServer.UseDefaultCredentials = false;
                    smtpServer.Credentials = new NetworkCredential(GlobalVariables.FromEmail, GlobalVariables.Password);

                    smtpServer.SendAsync(GlobalVariables.FromEmail, Email, GlobalVariables.Subject, GlobalVariables.Body.Replace("XXXXXX", newPassword), "xyz123d");

                    smtpServer.SendCompleted += SmtpServer_SendCompleted;

                    await Application.Current.MainPage.DisplayAlert("Sucesso", "E-mail com sua nova senha enviado.", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Usuário não registrado.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro ao redefinir a senha", $"{ex.Message}", "Ok");
            }
        }

        private async void SmtpServer_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if(user != null)
            {
                user.Password = newPassword;
                await userService.UpdateUser(user);
            }
            
            await Application.Current.MainPage.DisplayAlert("Sucesso", "E-mail enviado com sua nova senha.", "Ok");

            smtpServer.SendCompleted -= SmtpServer_SendCompleted;

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        #endregion
    }
}
