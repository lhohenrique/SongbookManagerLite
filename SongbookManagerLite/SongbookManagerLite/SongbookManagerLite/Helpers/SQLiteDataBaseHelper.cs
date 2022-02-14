using SongbookManagerLite.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerLite.Helpers
{
    public class SQLiteDataBaseHelper
    {
        readonly SQLiteAsyncConnection _db;

        public SQLiteDataBaseHelper(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Music>().Wait();
        }

        public Task<List<Music>> GetAllMusics()
        {
            return _db.Table<Music>().OrderByDescending(i => i.Id).ToListAsync();
        }

        public Task<Music> GetMusicById(int id)
        {
            return _db.Table<Music>().FirstAsync(i => i.Id == id);
        }

        public Task<int> InsertMusic(Music music)
        {
            return _db.InsertAsync(music);
        }

        public Task<List<Music>> UpdateMusic(Music music)
        {
            string sql = "UPDATE Music SET Name=?, Author=?, Key=?, Lyrics=?, Chords=? WHERE Id=?";

            return _db.QueryAsync<Music>(
                sql,
                music.Name,
                music.Author,
                music.Key,
                music.Lyrics,
                music.Chords,
                music.Id
            );
        }

        public Task<int> DeleteMusic(int id)
        {
            return _db.Table<Music>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Music>> SearchMusic(string searchText)
        {
            string sql = "SELECT * FROM Music WHERE NAME LIKE '%" + searchText + "%'";

            return _db.QueryAsync<Music>(sql);
        }

        public Task<int> DeleteAll()
        {
            return _db.Table<Music>().DeleteAsync(i => i.Id >= 0);
        }
    }
}
