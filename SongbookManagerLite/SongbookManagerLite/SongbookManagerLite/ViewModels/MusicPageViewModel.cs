using SongbookManagerLite.Helpers;
using SongbookManagerLite.Models;
using SongbookManagerLite.Services;
using SongbookManagerLite.Views;
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
    public class MusicPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }

        private MusicService musicService;

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

        private ObservableCollection<Music> musicList = new ObservableCollection<Music>();
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

        private string searchText = string.Empty;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
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

        #region [Commands]
        public ICommand NewMusicCommand
        {
            get => new Command(() =>
            {
                NewMusicAction();
                //Application.Current.MainPage.DisplayAlert("Alerta", "Nova música", "OK");
            });
        }

        public ICommand UpdateMusicListCommand
        {
            get => new Command(async () =>
            {
                try
                {
                    await LoggedUserHelper.UpdateLoggedUserAsync();

                    await UpdateMusicListAction();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possivel atualizar a lista de músicas", "OK");
                }
            });
        }

        public ICommand UpdateAllCommand
        {
            get => new Command(async () =>
            {
                // await UpdateLoggedUserAction();
                await UpdateMusicListAction();
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
            get => new Command(async () =>
            {
                await SearchAction();
            });
        }

        public ICommand ShareCommand
        {
            get => new Command(() =>
            {
                ShareAction();
            });
        }

        public ICommand MockMusicCommand
        {
            get => new Command(async () =>
            {
                var music1 = new Music()
                {
                    Name = "O Nome Da Paz",
                    UserEmail = "lho.henrique@gmail.com",
                    Author = "Resgate",
                    Key = "C",
                    Lyrics = "De que adianta estar vestido de branco\nE ter no rosto um sorriso amarelo\nSe a paz não é um estado de espírito\nSe por dentro há uma grande e interminável guerra\n\nA paz não é o que se encontra no mundo\nQue paz é essa que se arma prá guerra ?\nAonde está o fim da destruição ?\nAnsiedade quer vencer o desespero do coração\n\nO nome da paz, foi declarado na cruz\nO nome da paz é jesus\n\nOuço a voz que diz:\nA minha paz vos dou\nAgora tudo vai mudar",
                    Chords = "Am F              C G    Am\nde que adianta estar vestido de branco\n          F C       G\ne ter no rosto um sorriso amarelo\nAm F       C G\n   se a paz não é um estado de espírito\n F\nse por dentro há uma grande e interminável guerra\n\nInterlúdio: C G  Am G  C G  Am G\n\nAm F             C G\n   paz não é o que se encontra no mundo\nAm           F C          G\nque paz é essa que se arma pra guerra?\nAm F           C G\n   aonde está o fim da destruição?\n\n      F\nansiedade quer vencer o desespero do coração\n\nInterlúdio: C G  Am G  C G  Am G\n\nC G  Am F C G    Am F\n o nome da paz, foi declarado na cruz\nC         G Am    F\no nome da paz é Jesus\n\nInterlúdio: C G  Am G  C G  Am G\n\n         C Am\n ouço a voz que diz: minha paz vos dou\n           Em F   G C  G\nagora tudo vai mudar"
                };

                var music2 = new Music()
                {
                    Name = "Palavras",
                    UserEmail = "lho.henrique@gmail.com",
                    Author = "Resgate",
                    Key = "F#",
                    Lyrics = "Eu poderia ficar a vida inteira\nOuvindo a Tua voz, tão doce\nEu poderia ficar a vida inteira\nAo som de Tua palavras\nNem só de pão o homem viverá\nMas da palavra que sai da Tua boca\n\nEu poderia ficar a vida inteira\nOuvindo a Tua voz, tão doce\nEu poderia ficar a vida inteira\nAo som de Tua palavras\n\nComo uma lâmpada para os meus pés\nComo luz no meu caminho\nAs Tuas palavras\nQue alimentam\nO meu interior",
                    Chords = "[Intro]  G#m  F#  B9  C#9\n         G#m  F#  B9  C#9\n\nG#m  F#  B9    C#9                G#m   F#  B9  C#9\nEu____   poderia ficar a vida inteira\n  C#          D#m  Bbm7     B9   C#7/9\nOuvindo a Tua voz,\n                tão doce___\nG#m  F#  B9     C#9                G#m   F#  B9\nEu____    poderia ficar a vida inteira\n   C#9            D#m    Bbm7  B9  C#9\nAo som das Tuas palavras\n\nD#m       Bbm7  B9        D#m7\nNem só de pão o homem viverá\nB9                 C#9\nMas da palavra que sai da Tua boca\n\nG#m  F#  B9     C#9                G#m   F#  B9\nEu____    poderia ficar a vida inteira\n  C#9         D#m  Bbm7     B9   C#9\nOuvindo a Tua voz,\n                tão doce\n  G#m  F#  B9     C#9                G#m   F#  B9  C#9\nE eu____    poderia ficar a vida inteira\n                D#m    Bbm7  B  C#\nAo som de Tua palavras\n\n         G#m    Bbm  B            C#\nComo uma lâmpada    para os meus pés\n     D#m  Bbm7   B     C#\nComo luz___  no meu caminho\nB               G#m              C#  C#           G#m\nAh___ As Tuas palavras Que alimentam   o meu interior\n\n[Solo]  G#m  F#  B9  C#9\n        G#m  F#  B9  C#9\n        G#m  F#  B9  C#9\n        G#m  F#  B9  C#9\n\n(G#m  F#  B9  C#9 )\n\n         G#m    Bbm  B            C#\nComo uma lâmpada    para os meus pés\n     D#m  Bbm7   B     C#\nComo luz___  no meu caminho\nB               G#m              C#  C#           G#m\nAh___ As Tuas palavras Que alimentam   o meu interior\n\nG#m  F#  B9     C#9                G#m   F#  B9\nEu____    poderia ficar a vida inteira\n  C#9         D#m  Bbm7     B9   C#9\nOuvindo a Tua voz, (Tão doce)\n    G#m  F#     B9   C#9     G#m  F#     B9   C#9  F#\nTão doce(Tão doce).Tão doce(Tão doce____)"
                };

                var music3 = new Music()
                {
                    Name = "5:50 Am",
                    UserEmail = "lho.henrique@gmail.com",
                    Author = "Resgate",
                    Key = "G",
                    Lyrics = "Abro os olhos sob o mesmo teto, todo dia\nTudo outra vez\nAcordo, um tapa no relógio\nA mente tá vazia, são dez pra seis\n\nHoje a morte do meu ego tá fazendo aniversário\nSerá que eu vou chegar\nChegar ao fim de mais um calendário\nEu não sei! Eu não sei.....eu não sei...\nÉ tudo sempre igual\n\nDisseram que o Teu amor é novo a cada dia, eu pensei\nQuero ouvir a Tua voz\nFalar o que eu queria, são dez pra seis\n\nSe é pra Te servir e então matar aquela velha sede\nSe é pra Te seguir e nunca mais cair na mesma rede\nEu vou! Eu vou....eu vou...\nTe seguir... ",
                    Chords = "Intro 4x: G  Bm  Am7\n\nPrimeira Parte:\nEm                   D                C9\nAbro os olhos sob o mesmo teto, todo dia\n      G  Bm Am7\nTudo outra vez\n\nEm                    D\nAcordo, um tapa no relógio\nC                G Bm  Am7\nA mente tá vazia, são Dez pra seis\n\nSegunda Parte:\n C D9(11)\nHoje a morte do meu ego tá fazendo aniversário\n   C\nSerá que eu vou chegar\n                  D9(11)\nChegar ao fim de mais um calendário\n        G   D C9            G D           C9\nEu não sei!Eu não sei.....eu não sei...\nÉ tudo sempre igual\n       C D9(11)\n\n(Intro)\n\n(Primeira Parte - com variação na letra)\n   Em D                 C\nDisseram que o Teu amor é novo a cada dia,\n       G Bm7  Am7\nEu pensei\n\n Em               D\nQuero ouvir a Tua voz\n                  C G   Bm Am7\nFalar o que eu queria, são dez pra seis\n\n(Segunda Parte com variação)\n   C D9(11)\nSe é pra Te servir e então matar aquela velha sede\n  C                                     D9(11)\nSe é pra Te seguir e nunca mais cair na mesma rede\n    G D  C9 G  D C9\nEu vou!Eu vou....eu vou...\n\nTe seguir...\n\n(Solo)\n\n   C                                      D9(11)\nSe é pra Te servir e então matar aquela velha sede\n  C                                     D9(11)\nSe é pra Te seguir e nunca mais cair na mesma rede\n    G D  C9 G  D C9\nEu vou!Eu vou....eu vou\n\nTe seguir"
                };

                //await App.Database.InsertMusic(music1);
                //await App.Database.InsertMusic(music2);
                //await App.Database.InsertMusic(music3);

                await musicService.InsertMusic(music1);
                await musicService.InsertMusic(music2);
                await musicService.InsertMusic(music3);
            });
        }

        public ICommand ClearCommand
        {
            get => new Command(async () =>
            {
                await App.Database.DeleteAll();
            });
        }
        #endregion

        public MusicPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            musicService = new MusicService();

            // Update logged user
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
            try
            {
                var result = await Application.Current.MainPage.DisplayAlert("Tem certeza?", $"A música '{music.Name}' será removida da sua lista.", "Sim", "Não");

                if (result)
                {
                    //await App.Database.DeleteMusic(music.Id);
                    await musicService.DeleteMusic(music);
                }

                await UpdateMusicListAction();
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task UpdateMusicListAction()
        {
            try
            {
                if (IsUpdating)
                {
                    return;
                }

                IsUpdating = true;

                //List<Music> musicListUpdated = await App.Database.GetAllMusics();
                var userEmail = LoggedUserHelper.GetEmail();
                List<Music> musicListUpdated = await musicService.GetMusicsByUser(userEmail);

                MusicList.Clear();

                musicListUpdated.ForEach(i => MusicList.Add(i));
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

        private async Task SearchAction()
        {
            try
            {
                if (IsUpdating)
                {
                    return;
                }

                isUpdating = true;

                //List<Music> musicListUpdated = await App.Database.SearchMusic(SearchText);
                var userEmail = LoggedUserHelper.GetEmail();
                List<Music> musicListUpdated = await musicService.SearchMusic(SearchText, userEmail);

                MusicList.Clear();

                musicListUpdated.ForEach(i => MusicList.Add(i));
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsUpdating = false;
            }
        }

        private void ShareAction()
        {
            Navigation.PushAsync(new SharePage());
        }
        #endregion

        #region [Private Methods]
        #endregion
    }
}
