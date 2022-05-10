using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SongbookManagerLite.Services
{
    public class MusicService
    {
        FirebaseClient client;

        public MusicService()
        {
            client = new FirebaseClient("https://songbookmanagerlite-default-rtdb.firebaseio.com/");
        }
    }
}
