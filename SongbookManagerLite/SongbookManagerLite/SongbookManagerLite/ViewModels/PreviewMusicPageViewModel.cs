﻿using SongbookManagerLite.Models;
using SongbookManagerLite.Services;
using SongbookManagerLite.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SongbookManagerLite.ViewModels
{
    public class PreviewMusicPageViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Music music;
        private MusicService musicService;

        #region [Properties]
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

        private string key;
        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Key"));
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
        #endregion

        #region [Commands]
        public ICommand ShowMusicDetailsCommand
        {
            get => new Command(() =>
            {
                ShowMusicDetailsAction();
            });
        }

        public ICommand EditMusicCommand
        {
            get => new Command(() =>
            {
                EditMusicAction();
            });
        }

        public ICommand RemoveMusicCommand
        {
            get => new Command(async () =>
            {
                await RemoveMusicAction();
            });
        }
        #endregion

        public PreviewMusicPageViewModel(INavigation navigation, Music music)
        {
            Navigation = navigation;
            this.music = music;

            musicService = new MusicService();
        }

        #region [Actions]
        private void ShowMusicDetailsAction()
        {
            if(music != null)
            {
                Name = music.Name;
                Author = music.Author;
                Key = music.Key;
                Lyrics = music.Lyrics;
                Chords = music.Chords;
            }
        }

        private void EditMusicAction()
        {
            if(music != null)
            {
                Navigation.PushAsync(new AddEditMusicPage(music));
            }
        }

        private async Task RemoveMusicAction()
        {
            if(music != null)
            {
                try
                {
                    var result = await Application.Current.MainPage.DisplayAlert("Tem certeza?", $"A música {music.Name} será removida da sua lista.", "Sim", "Não");

                    if (result)
                    {
                        //await App.Database.DeleteMusic(music.Id);
                        await musicService.DeleteMusic(music);
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            }

            await Navigation.PopAsync();
        }
        #endregion
    }
}
