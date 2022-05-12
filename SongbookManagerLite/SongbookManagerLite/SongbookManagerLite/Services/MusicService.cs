using Firebase.Database;
using Firebase.Database.Query;
using SongbookManagerLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerLite.Services
{
    public class MusicService
    {
        FirebaseClient client;

        public MusicService()
        {
            client = new FirebaseClient("https://songbookmanagerlite-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Music>> GetMusics()
        {
            var musics = (await client.Child("Musics").OnceAsync<Music>())
                .Select(item => new Music
                {
                    Name = item.Object.Name,
                    Author = item.Object.Author,
                    Key = item.Object.Key,
                    Lyrics = item.Object.Lyrics,
                    Chords = item.Object.Chords,
                    UserEmail = item.Object.UserEmail
                }).ToList();

            return musics;
        }

        public async Task<List<Music>> GetMusicsByUser(string userEmail)
        {
            var musics = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
            {
                Name = item.Object.Name,
                Author = item.Object.Author,
                Key = item.Object.Key,
                Lyrics = item.Object.Lyrics,
                Chords = item.Object.Chords,
                UserEmail = item.Object.UserEmail
            }).Where(m => m.UserEmail.Equals(userEmail)).ToList();

            return musics;
        }

        public async Task<bool> InsertMusic(Music music)
        {
            await client.Child("Musics").PostAsync(music);

            return true;
            
        }

        public async Task UpdateMusic(Music music)
        {
            var musicToUpdate = (await client.Child("Musics").OnceAsync<Music>())
                                                .Where(m => m.Object.Name.Equals(music.Name) && m.Object.UserEmail.Equals(music.UserEmail)).FirstOrDefault();

            await client.Child("Musics").Child(musicToUpdate.Key).PutAsync(music);
        }

        public async Task DeleteMusic(Music music)
        {
            var musicToDelete = (await client.Child("Musics").OnceAsync<Music>())
                                                .Where(m => m.Object.Name.Equals(music.Name) && m.Object.UserEmail.Equals(music.UserEmail)).FirstOrDefault();

            await client.Child("Musics").Child(musicToDelete.Key).DeleteAsync();
        }

        public async Task<List<Music>> SearchMusic(string searchText, string userEmail)
        {
            var musics = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
            {
                Name = item.Object.Name,
                Author = item.Object.Author,
                Key = item.Object.Key,
                Lyrics = item.Object.Lyrics,
                Chords = item.Object.Chords,
                UserEmail = item.Object.UserEmail
            }).Where(m => m.UserEmail.Equals(userEmail) && m.Name.Contains(searchText)).ToList();

            return musics;
        }

        public async Task DeleteAll()
        {
            await client.Child("Musics").DeleteAsync();
        }
    }
}
