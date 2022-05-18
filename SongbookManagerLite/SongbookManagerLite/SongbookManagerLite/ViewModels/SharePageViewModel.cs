using SongbookManagerLite.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SongbookManagerLite.ViewModels
{
    public class SharePageViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #region [Properties]
        private ObservableCollection<User> userList = new ObservableCollection<User>();
        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set { userList = value; }
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
        #endregion

        public SharePageViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        #region [Actions]
        private void ShareAction()
        {

        }
        #endregion
    }
}
