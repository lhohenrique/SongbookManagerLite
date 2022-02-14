﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SongbookManagerLite.Models
{
    public class Music
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Key { get; set; }
        public string Lyrics { get; set; }
        public string Chords { get; set; }
        public string Tipo { get; set; }
    }
}
