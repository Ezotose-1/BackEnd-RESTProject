using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BackEnd_RESTProject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BackEnd_RESTProject.Models;
using BackEnd_RESTProject.Services;
using BackEnd_RESTProject.Helpers;
using BackEnd_RESTProject.Data;
using System.Linq;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;

        public UsersController(
            Context context,
            IUserService userService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
            Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

            // Function too see your own profile
        /// <summary>
        /// See your profile
        /// </summary>
        [HttpGet("GetProfile")]
        public IActionResult GetProfile()
        {
            int id = int.Parse(User.Identity.Name);
            var user = _userService.GetById(id);
            var rates = _context.Rate.Where(x => x.User_ToId == user.Id).ToList();
            var jobs = _context.Job.Where(x => ((User.IsInRole("Employer") || User.IsInRole("Admin")) ? x.EmployerID == id : x.CandidatID == id)).ToList();
            var ratesDTOLst = new List<RateDTO>();

            foreach (var rate in rates)
            {
                ratesDTOLst.Add( new RateDTO {
                    JobDescription = "",
                    Stars   = rate.Stars,
                    Comment = rate.Comment
                });
            }
            for(int i = 0; i < ratesDTOLst.Count; i++)
            {
                ratesDTOLst[i].JobDescription = jobs[i].Description;
            }

            var model = new UserProfileDTO {
                Id          = user.Id,
                FirstName   = user.FirstName,
                LastName    = user.LastName,
                Username    = user.Username,
                Skillset    = user.Skillset,
                Avaible     = user.Avaible,
                Role        = user.Role,
                Rates       = ratesDTOLst,
            };
            return Ok(model);
        }


        [Authorize(Roles= Role.Employer)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            //Finding who is logged in
            int logged_in_user = int.Parse(User.Identity.Name);

            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            //Rejecting access if the logged in user is not same as the user updating information
            if (logged_in_user != id)
            {
                return BadRequest(new { message = "Access Denied" });
            }

            try
            {
                // update user 
                _userService.Update(user, model.CurrentPassword, model.NewPassword, model.ConfirmNewPassword, model.Avaible, model.Skillset);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


            // Function for candidat to advertise himself to get first on a list
        /// <summary>
        /// Candidat : Advertise yourself to get first on the candidat's list
        /// </summary>
        [HttpPut("Advertise/{Advertise}")]
        public IActionResult Avertise(bool Advertise)
        {
            int id = int.Parse(User.Identity.Name);
            var user = _context.User.ToList().Find(x => x.Id == id);
            user.Advertise = Advertise;
            _context.User.Update(user);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize(Roles= Role.Admin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = _userService.ResetPassword(model.Username, model.key, model.newpassword);

            if (user == null)
                return BadRequest(new { message = "Username or key is incorrect" });

           

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password
            });
        }
    }
}
