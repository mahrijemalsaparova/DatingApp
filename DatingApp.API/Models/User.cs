namespace DatingApp.API.Models
{
    public class User
    {
        public int  Id { get; set; }
        public string Username { get; set; }
        //storing password in database is like that
        public byte[] PasswordHash { get; set; }
        //passwordsalt act likee a key
        public byte[] PasswordSalt { get; set; }

    }
}