using SongbookManagerLite.Helpers;
using SongbookManagerLite.Views;
using System;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SongbookManagerLite
{
    public partial class App : Application
    {
        private static SQLiteDataBaseHelper database;
        public static SQLiteDataBaseHelper Database
        {
            get
            {
                if(database == null)
                {
                    database = new SQLiteDataBaseHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamSongbookManagerLite.db3"));
                }

                return database;
            }
        }


        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            var loggedUserEmail = Preferences.Get("Email", string.Empty);
            if (string.IsNullOrEmpty(loggedUserEmail))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new MainPage();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
