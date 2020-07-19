using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password); //Register user
        Task<User> Login(string username, string password); //Login user
        Task<bool> UserExists(string username); //Check if or not user is exist

    }
}