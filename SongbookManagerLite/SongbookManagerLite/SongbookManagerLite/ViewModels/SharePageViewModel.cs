using SongbookManagerLite.Helpers;
using SongbookManagerLite.Models;
using SongbookManagerLite.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SongbookManagerLite.ViewModels
{
    public class SharePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        
        private UserService userService;

        #region [Properties]
        private ObservableCollection<User> userList = new ObservableCollection<User>();
        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set { userList = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                HandleShareButton();
            }
        }

        private bool isShareEnabled = false;
        public bool IsShareEnabled
        {
            get { return isShareEnabled; }
            set
            {
                isShareEnabled = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsShareEnabled"));
            }
        }

        private bool isUpdating = false;
        public bool IsUpdating
        {
            get { return isUpdating; }
            set
            {
                isUpdating = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsUpdating"));
            }
        }
        #endregion

        #region [Command]
        public ICommand ShareCommand
        {
            get => new Command(() =>
            {
                ShareAction();
            });
        }

        public ICommand UpdateUserListCommand
        {
            get => new Command(async () =>
            {
                await UpdateUserListAction();
            });
        }
        #endregion

        public SharePageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            userService = new UserService();
        }

        #region [Actions]
        private async Task ShareAction()
        {
            try
            {
                var userToShare = await userService.GetUser(Email);

                if (userToShare == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", $"O usuário '{Email}' não foi encontrado.", "Ok");
                }
                else if (!string.IsNullOrEmpty(userToShare.SharedList))
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", $"O usuário já está acessando uma lista de músicas compartilhada.", "Ok");
                }
                else
                {
                    userToShare.SharedList = LoggedUserHelper.GetEmail();
                    await userService.UpdateUser(userToShare);

                    await Application.Current.MainPage.DisplayAlert("Sucesso", $"Lista compartilhada com sucesso.", "Ok");

                    await UpdateUserListAction();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task UpdateUserListAction()
        {
            try
            {
                if (IsUpdating)
                {
                    return;
                }

                IsUpdating = true;

                var userEmail = LoggedUserHelper.GetEmail();
                List<User> userListUpdated = await userService.GetSharedUsers(userEmail);

                UserList.Clear();

                userListUpdated.ForEach(i => UserList.Add(i));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsUpdating = false;
            }
        }
        #endregion

        #region [Private Methods]
        public void HandleShareButton()
        {
            IsShareEnabled = !string.IsNullOrEmpty(Email);   
        }
        #endregion
    }
}
