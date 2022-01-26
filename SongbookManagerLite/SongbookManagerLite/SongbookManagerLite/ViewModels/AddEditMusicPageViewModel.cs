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
    public class AddEditMusicPageViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Music music;

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
            get => new Command(() =>
            {
                SaveMusicAction();
            });
        }
        #endregion

        public AddEditMusicPageViewModel(INavigation navigation, Music music)
        {
            Navigation = navigation;

            this.music = music;
        }

        #region [Actions]
        public void SaveMusicAction()
        {
            // Edit
            if(music != null)
            {

            }
            else // Save
            {

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
        }
        #endregion
    }
}
