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
    public class MusicPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region [Properties]
        private bool isAddEditMusic = false;
        public bool IsAddEditMusic
        {
            get { return isAddEditMusic; }
            set 
            {
                isAddEditMusic = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsAddEditMusic"));
            }
        }

        private bool isShowMusicList = true;
        public bool IsShowMusicList
        {
            get { return isShowMusicList; }
            set
            {
                isShowMusicList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsShowMusicList"));
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

        public ICommand SaveMusicCommand
        {
            get => new Command(() =>
            {
                SaveMusicAction();
            });
        }

        public ICommand CancelMusicCommand
        {
            get => new Command(() =>
            {
                CancelMusicAction();
            });
        }
        #endregion

        public MusicPageViewModel()
        {
            KeyList = new ObservableCollection<string>(){"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};

            MusicList = new ObservableCollection<Music>();

            var music1 = new Music() { Name = "Music 1", Author = "Fulano 1", Key = "C"};
            var music2 = new Music() { Name = "Music 2", Author = "Fulano 2", Key = "C#"};
            var music3 = new Music() { Name = "Music 3", Author = "Fulano 3", Key = "D"};
            var music4 = new Music() { Name = "Music 4", Author = "Fulano 4", Key = "G#"};

            MusicList.Add(music1);
            MusicList.Add(music2);
            MusicList.Add(music3);
            MusicList.Add(music4);
        }

        #region [Private Methods]
        private void NewMusicAction()
        {
            ClearMusicFields();
            HandleMusicPageState();
        }

        private void SaveMusicAction()
        {
            var newMusic = new Music()
            {
                Name = this.Name,
                Author = this.Author,
                Key = this.SelectedKey,
                Lyrics = this.Lyrics,
                Chords = this.Chords
            };

            MusicList.Add(newMusic);

            ClearMusicFields();
            HandleMusicPageState();
        }

        private void CancelMusicAction()
        {
            ClearMusicFields();
            HandleMusicPageState();
        }

        private void HandleMusicPageState()
        {
            if (IsAddEditMusic)
            {
                IsAddEditMusic = false;
                IsShowMusicList = !IsAddEditMusic;
            }
            else
            {
                IsAddEditMusic = true;
                IsShowMusicList = !IsAddEditMusic;
            }
        }

        private void ClearMusicFields()
        {
            Name = string.Empty;
            Author = string.Empty;
            SelectedKey = string.Empty;
            Lyrics = string.Empty;
            Chords = string.Empty;
        }
        #endregion
    }
}
