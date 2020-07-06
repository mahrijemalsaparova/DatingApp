using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    //QUERING OUR DATABASE BY ENTITYFRAMEWORK
    public class AuthRepository : IAuthRepository
    {
        ////////INJECTION OF  OUR DATABASE////////////
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        { 
            _context = context;

        }
        /////////////LOGIN METHOD//////////////////
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user == null)
               return null;
//parametrenin içindeki userdan aaldığı password ve doğru veya yanlış dödüren metoddur.
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            //if doesnt match returnn null demek.
              return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //key
            { 
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for(int i = 0 ; i > computedHash.Length; i++)
            {//is not equal to
                if (computedHash[i] != passwordHash[i]) return false; //if these are two match means password is correct
            }
            }
            return true;
        }
        ////REGISTER METHOD////
        public async Task<User> Register(User user, string password)
        {

            //converting password into passwordHash and the passwordSalt
            byte[] passwordHash, passwordSalt;      //declaring variables
            CreatePasswordHash(password, out passwordHash,out passwordSalt);  //The out is a keyword in C# which is used for the passing the arguments to methods as a reference type. It is generally used when a method returns multiple values.


            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
       
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
 //////////////////LOGIC FOR THE USEREXIST////////////////////////
        public async Task<bool> UserExists(string username)
        {
           if(await _context.Users.AnyAsync(x => x.Username == username))
             return true;
            
          return false;
        }
////////////////////////////////////////////////////////////////
    }
}