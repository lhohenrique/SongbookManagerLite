using SongbookManagerLite.Models;
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

        public ICommand UpdateMusicList
        {
            get => new Command(() =>
            {
                //UpdateMusicListAction
            });
        }

        public ICommand SelectMusicCommand
        {
            get => new Command(() =>
            {
                CancelMusicAction();
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
        #endregion

        public MusicPageViewModel()
        {
            KeyList = new ObservableCollection<string>(){"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};

            MusicList = new ObservableCollection<Music>();

            var music1 = new Music() { Id = 0, Name = "Music 1", Author = "Fulano 1", Key = "C", Lyrics = "Bla bla bla bla", Chords = "C C C C"};
            var music2 = new Music() { Id = 1, Name = "Music 2", Author = "Fulano 2", Key = "C#", Lyrics = "Bla bla bla bla", Chords = "C C C C" };
            var music3 = new Music() { Id = 2, Name = "Music 3", Author = "Fulano 3", Key = "D", Lyrics = "Bla bla bla bla", Chords = "C C C C" };
            var music4 = new Music() { Id = 3, Name = "Music 4", Author = "Fulano 4", Key = "G#", Lyrics = "Bla bla bla bla", Chords = "C C C C" };

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
            if(this.Id == 0)
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
            }
            else
            {
                // Update music according to Id
            }
            
            ClearMusicFields();
            HandleMusicPageState();
        }

        private void CancelMusicAction()
        {
            ClearMusicFields();
            HandleMusicPageState();
        }

        private void EditMusicAction(Music music)
        {
            ClearMusicFields();

            Id = music.Id;
            Name = music.Name;
            Author = music.Author;
            SelectedKey = music.Key;
            Lyrics = music.Lyrics;
            Chords = music.Chords;

            HandleMusicPageState();
        }

        private async Task RemoveMusicAction(Music music)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Tem certeza?", $"A música {music.Name} será removida da sua lista.", "Sim", "Não");

            if (result)
            {
                MusicList.Remove(music);
            }
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
