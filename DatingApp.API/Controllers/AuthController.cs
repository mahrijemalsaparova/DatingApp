using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extentions.Congiguration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")] //api/Auth
    [ApiController]
    public class AuthController : ControllerBase
    { //injection our new IAuthRepository
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        

        [HttpPost("register")] //Httppost method

        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //validate request / burada kullanıcıdan veri aldığımız için önce validation (onaylama) 
            //işlemi yapılmalıdır.

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower(); // user'dan gelen ismini küçük yazdırmak için. Karışıklık olamamsı için.
           
            //Şimdi girilen (kullanıcının girdiği) ismini daha önce alınıp alınmadığını kontrol edicez.

            if(await _repo.UserExists(userForRegisterDto.Username))
            return BadRequest("User already exist");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201); //HTTP Status 201 indicates that as a result of HTTP POST request, 
                                    //one or more new resources have been successfully created on server.
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)        
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            
            if(userForLogin == null)
              return Unauthorized();

//starting build token
            var claims =  new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
//controlling the token is valid token or not

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.
            GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signutere);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);  


            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}