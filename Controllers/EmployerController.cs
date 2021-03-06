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
    public class EmployerController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;

        public EmployerController(
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

        /// <summary>
        /// List all candidates.
        /// </summary>
        [Authorize(Roles= Role.Employer)]
        [HttpGet("SeeAllCandidats")]
        public IActionResult GetAll()
        {
            var users = _context.User.ToList().Where(x => x.Role == "Candidat").OrderByDescending(x => x.Advertise);
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        /// <summary>
        /// See candidate profile.
        /// </summary>
        [Authorize(Roles= Role.Employer)]
        [HttpGet("SeeACandidat/{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null || user.Role != "Candidat")
                return Ok("Bad request");

            var rates = _context.Rate.Where(x => x.User_ToId == user.Id).ToList();
            var ratesDTOLst = new List<RateDTO>();

            foreach (var rate in rates)
            {
                ratesDTOLst.Add( new RateDTO {
                    Stars   = rate.Stars,
                    Comment = rate.Comment
                });
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

       
    }
}
