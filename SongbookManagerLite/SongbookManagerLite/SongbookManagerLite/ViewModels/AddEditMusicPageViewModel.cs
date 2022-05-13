using SongbookManagerLite.Models;
using SongbookManagerLite.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

            this.music = music;
            oldName = music.Name;
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
                    var musicService = new MusicService();
                    await musicService.UpdateMusic(music, oldName);
                }
                else // Save new music
                {
                    var userEmail = Preferences.Get("Email", string.Empty);

                    // TODO: Não utilizar o email logado, usar o email do LoggedUserHelper pq o usuário pode estar inserindo em uma lista compartilhada
                    var newMusic = new Music()
                    {
                        Name = Name,
                        UserEmail = userEmail,
                        Author = Author,
                        Key = SelectedKey,
                        Lyrics = Lyrics,
                        Chords = Chords
                    };

                    //await App.Database.InsertMusic(newMusic);
                    var musicService = new MusicService();
                    await musicService.InsertMusic(newMusic);
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
        public void PopulateMusicFields()
        {
            if(music != null)
            {
                Id = music.Id;
                Name = music.Name;
                Author = music.Author;
                SelectedKey = music.Key;
                Lyrics = music.Lyrics;
                Chords = music.Chords;
            }
            else
            {
                Id = 0;
                Name = string.Empty;
                Author = string.Empty;
                SelectedKey = string.Empty;
                Lyrics = string.Empty;
                Chords = string.Empty;
            }
        }
        #endregion
    }
}
