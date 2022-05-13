using SongbookManagerLite.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace SongbookManagerLite.Helpers
{
    public class LoggedUserHelper
    {
        private static User loggedUser;
        public static User LoggedUser
        {
            get
            {
                if (loggedUser == null)
                {
                    loggedUser = new User();
                }

                return loggedUser;
            }
        }

        public static void UpdateLoggedUser(User user)
        {
            loggedUser = user;
        }

        public static bool HasSharedList()
        {
            return !string.IsNullOrEmpty(LoggedUser.SharedList);
        }

        public static string GetSharedId()
        {
            if (HasSharedList())
            {
                return LoggedUser.SharedList;
            }
            else
            {
                return LoggedUser.Email;
            }
        }
    }
}
