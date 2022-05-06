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
            return LoggedUser.GroupId > 0;
        }

        public static int GetSharedId()
        {
            if (HasSharedList())
            {
                return LoggedUser.GroupId;
            }
            else
            {
                return LoggedUser.Id;
            }
        }
    }
}
