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
    public class UserService
    {
        FirebaseClient client;

        public UserService()
        {
            client = new FirebaseClient("https://songbookmanagerlite-default-rtdb.firebaseio.com/");
        }

        public async Task<bool> IsUserExists(string name)
        {
            var user = (await client.Child("Users").OnceAsync<User>()).Where(u => u.Object.Name == name).FirstOrDefault();

            return user != null;
        }


        public async Task<bool> RegisterUSer(string name, string email, string password)
        {
            if(await IsUserExists(name) == false)
            {
                await client.Child("Users")
                    .PostAsync(new User()
                    { 
                        Name = name,
                        Email = email,
                        Password = password
                    });
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> LoginUser(string name, string password)
        {
            var user = (await client.Child("Users").OnceAsync<User>()).Where(u => u.Object.Name == name && u.Object.Password == password);

            return user != null;
        }
    }
}
