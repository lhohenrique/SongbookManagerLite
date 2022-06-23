using SongbookManagerLite.Resx;
using System;
using System.Collections.Generic;
using System.Text;

namespace SongbookManagerLite.Helpers
{
    public static class GlobalVariables
    {
        #region [Email]
        public static string Password = "368869Lho";
        public static string FromEmail = "songbookmanagerapp@gmail.com";
        public static string Subject = AppResources.NewPasswordEmailSubject;
        public static string Body = AppResources.NewPasswordEmailBody;
        #endregion
    }
}
