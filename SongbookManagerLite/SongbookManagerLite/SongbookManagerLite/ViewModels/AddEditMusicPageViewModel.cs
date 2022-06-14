﻿using SongbookManagerLite.Helpers;
using SongbookManagerLite.Models;
using SongbookManagerLite.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SongbookManagerLite.ViewModels
{
    public class AddEditMusicPageViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Music music;
        private string oldName;
        private MusicService musicService;
        private UserService userService;
        private KeyService keyService;

        #region [Properties]
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private string author;
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Author"));
            }
        }

        private string selectedKey;
        public string SelectedKey
        {
            get { return selectedKey; }
            set
            {
                selectedKey = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedKey"));
            }
        }

        private string lyrics;
        public string Lyrics
        {
            get { return lyrics; }
            set
            {
                lyrics = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Lyrics"));
            }
        }

        private string chords;
        public string Chords
        {
            get { return chords; }
            set
            {
                chords = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Chords"));
            }
        }

        private ObservableCollection<string> keyList = new ObservableCollection<string>(){"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};
        public ObservableCollection<string> KeyList
        {
            get { return keyList; }
            set { keyList = value; }
        }

        private ObservableCollection<UserKey> userList = new ObservableCollection<UserKey>();
        public ObservableCollection<UserKey> UserList
        {
            get { return userList; }
            set { userList = value; }
        }

        private bool hasSingers = false;
        public bool HasSingers
        {
            get { return hasSingers; }
            set
            {
                hasSingers = value;
                PropertyChanged(this, new PropertyChangedEventArgs("HasSingers"));
            }
        }
        #endregion

        #region [Commands]
        public ICommand SaveMusicCommand
        {
            get => new Command(async () =>
            {
                await SaveMusicAction();
            });
        }
        #endregion

        public AddEditMusicPageViewModel(INavigation navigation, Music music)
        {
            Navigation = navigation;

            musicService = new MusicService();
            keyService = new KeyService();
            userService = new UserService();

            this.music = music;
            oldName = music?.Name;
        }

        #region [Actions]
        public async Task SaveMusicAction()
        {
            try
            {
                // Edit music
                if (music != null)
                {
                    music.Name = Name;
                    music.Author = Author;
                    music.Key = SelectedKey;
                    music.Lyrics = Lyrics;
                    music.Chords = Chords;

                    //await App.Database.UpdateMusic(music);
                    await musicService.UpdateMusic(music, oldName);

                    foreach (UserKey userKey in UserList)
                    {
                        if (!string.IsNullOrEmpty(userKey.Key))
                        {
                            var isUserKeyExists = await keyService.IsUserKeyExists(userKey);
                            if (isUserKeyExists)
                            {
                                await keyService.UpdateKey(userKey);
                            }
                            else
                            {
                                await keyService.InsertKey(userKey);
                            }
                        }
                    }
                }
                else // Save new music
                {
                    var newMusic = new Music()
                    {
                        Name = Name,
                        UserEmail = LoggedUserHelper.GetEmail(),
                        Author = Author,
                        Key = SelectedKey,
                        Lyrics = Lyrics,
                        Chords = Chords,
                        CreationDate = DateTime.Now
                    };

                    //await App.Database.InsertMusic(newMusic);
                    await musicService.InsertMusic(newMusic);

                    foreach (UserKey userKey in UserList)
                    {
                        if (!string.IsNullOrEmpty(userKey.Key))
                        {
                            userKey.MusicName = newMusic.Name;
                            await keyService.InsertKey(userKey);
                        }
                    }
                }

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
        #endregion

        #region [Public Methods]
        public async Task PopulateMusicFieldsAsync()
        {
            try
            {
                var musicOwner = LoggedUserHelper.GetEmail();

                if (music != null)
                {
                    Id = music.Id;
                    Name = music.Name;
                    Author = music.Author;
                    SelectedKey = music.Key;
                    Lyrics = music.Lyrics;
                    Chords = music.Chords;

                    var sharedUsers = await userService.GetSingers(musicOwner);
                    var usersKeys = await keyService.GetKeysByOwner(musicOwner, Name);

                    foreach (User user in sharedUsers)
                    {
                        if (!usersKeys.Exists(u => u.UserEmail.Equals(user.Email)))
                        {
                            usersKeys.Add(new UserKey()
                            {
                                MusicName = Name,
                                UserName = user.Name,
                                UserEmail = user.Email,
                                MusicOwner = musicOwner
                            });
                        }
                    }

                    usersKeys.ForEach(i => UserList.Add(i));
                }
                else
                {
                    Id = 0;
                    Name = string.Empty;
                    Author = string.Empty;
                    SelectedKey = string.Empty;
                    Lyrics = string.Empty;
                    Chords = string.Empty;

                    var sharedUsers = await userService.GetSingers(musicOwner);
                    sharedUsers.ForEach(u => UserList.Add(new UserKey()
                    {
                        UserName = u.Name,
                        UserEmail = u.Email,
                        MusicOwner = musicOwner
                    }));
                }

                HasSingers = UserList.Any();
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possivel carregar a página", "OK");
            }
        }
        #endregion

        #region [Private Methods]
        #endregion
    }
}
