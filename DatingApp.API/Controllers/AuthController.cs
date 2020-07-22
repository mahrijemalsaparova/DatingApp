using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")] //api/Auth
    [ApiController]
    public class AuthController : ControllerBase
    { //injection our new IAuthRepository
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
           _mapper = mapper;
            _repo = repo;
            _config = config;
        }


        [HttpPost("register")] //Httppost method

        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {//validate request / burada kullanıcıdan veri aldığımız için önce validation (onaylama) 
            //işlemi yapılmalıdır.
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower(); // user'dan gelen ismini küçük yazdırmak için. Karışıklık olamamsı için.
                                                                                 //Şimdi girilen (kullanıcının girdiği) ismini daha önce alınıp alınmadığını kontrol edicez.
            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("User already exist");
            //creating user
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201); //HTTP Status 201 indicates that as a result of HTTP POST request, 
                                    //one or more new resources have been successfully created on server.
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            //starting build token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            //controlling the token is valid token or not

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDto>(userFromRepo);


            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });


            /*  var token = new JwtSecurityToken(
                  issuer: "localhost",
                  audience: "localhost",
                  claims: claims,
                  expires: DateTime.Now.AddDays(1),
                  signingCredentials: creds
              );

              return Ok(new
              {
                  token = new JwtSecurityTokenHandler().WriteToken(token)
              });
              */
        }
    }
}