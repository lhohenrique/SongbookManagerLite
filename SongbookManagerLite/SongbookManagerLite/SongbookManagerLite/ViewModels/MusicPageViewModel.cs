﻿using SongbookManagerLite.Models;
using SongbookManagerLite.Views;
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
    public class MusicPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }


        #region [Properties]
        private bool isPreviewMusic = false;
        public bool IsPreviewMusic
        {
            get { return isPreviewMusic; }
            set
            {
                isPreviewMusic = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsPreviewMusic"));
            }
        }

        private Music selectedMusic;
        public Music SelectedMusic
        {
            get => selectedMusic;
            set
            {
                selectedMusic = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedMusic"));
                SelectedMusicAction();
            }
        }

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
            get { return name; }
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

        private string previewName;
        public string PreviewName
        {
            get { return previewName; }
            set
            {
                previewName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PreviewName"));
            }
        }

        private string previewAuthor;
        public string PreviewAuthor
        {
            get { return previewAuthor; }
            set
            {
                previewAuthor = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PreviewAuthor"));
            }
        }

        private string previewKey;
        public string PreviewKey
        {
            get { return previewKey; }
            set
            {
                previewKey = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PreviewKey"));
            }
        }

        private string previewLyrics;
        public string PreviewLyrics
        {
            get { return previewLyrics; }
            set
            {
                previewLyrics = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PreviewLyrics"));
            }
        }

        private string previewChords;
        public string PreviewChords
        {
            get { return previewChords; }
            set
            {
                previewChords = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PreviewChords"));
            }
        }

        private ObservableCollection<Music> musicList;
        public ObservableCollection<Music> MusicList
        {
            get { return musicList; }
            set { musicList = value; }
        }

        private ObservableCollection<string> keyList;
        public ObservableCollection<string> KeyList
        {
            get { return keyList; }
            set { keyList = value; }
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SearchText"));
            }
        }
        #endregion

        #region [Commands]
        public ICommand NewMusicCommand
        {
            get => new Command(() =>
            {
                NewMusicAction();
                //Application.Current.MainPage.DisplayAlert("Alerta", "Nova música", "OK");
            });
        }

        public ICommand UpdateMusicList
        {
            get => new Command(() =>
            {
                //UpdateMusicListAction
            });
        }

        public ICommand EditMusicCommand
        {
            get => new Command<Music>((Music music) =>
            {
                EditMusicAction(music);
            });
        }

        public ICommand RemoveMusicCommand
        {
            get => new Command<Music>(async (Music music) =>
            {
                await RemoveMusicAction(music);
            });
        }

        public ICommand SearchCommand
        {
            get => new Command(() =>
            {
                SearchAction();
            });
        }
        #endregion

        public MusicPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            MusicList = new ObservableCollection<Music>();

            var music1 = new Music() { Id = 0, Name = "Music 1", Author = "Fulano 1", Key = "C", Lyrics = "Bla bla bla bla", Chords = "C C C C"};
            var music2 = new Music() { Id = 1, Name = "Music 2", Author = "Fulano 2", Key = "C#", Lyrics = "Bla bla bla bla", Chords = "C C C C" };
            var music3 = new Music() { Id = 2, Name = "Music 3", Author = "Fulano 3", Key = "D", Lyrics = "Bla bla bla bla", Chords = "C C C C" };
            var music4 = new Music() { Id = 3, Name = "Music 4", Author = "Fulano 4", Key = "G#", Lyrics = "Bla bla bla bla", Chords = "C C C C" };

            MusicList.Add(music1);
            MusicList.Add(music2);
            MusicList.Add(music3);
            MusicList.Add(music4);
            MusicList.Add(music1);
            MusicList.Add(music2);
            MusicList.Add(music3);
            MusicList.Add(music4);
            MusicList.Add(music1);
            MusicList.Add(music2);
            MusicList.Add(music3);
            MusicList.Add(music4);
        }

        #region [Actions]
        private void SelectedMusicAction()
        {
            if(SelectedMusic != null)
            {
                Navigation.PushAsync(new PreviewMusicPage(selectedMusic));
            }
        }

        private void NewMusicAction()
        {
            Navigation.PushAsync(new AddEditMusicPage(null));
        }

        private void EditMusicAction(Music music)
        {           
            Navigation.PushAsync(new AddEditMusicPage(music));
        }

        private async Task RemoveMusicAction(Music music)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Tem certeza?", $"A música {music.Name} será removida da sua lista.", "Sim", "Não");

            if (result)
            {
                MusicList.Remove(music);
            }
        }

        private void SearchAction()
        {

        }
        #endregion

        #region [Private Methods]
        #endregion
    }
}
