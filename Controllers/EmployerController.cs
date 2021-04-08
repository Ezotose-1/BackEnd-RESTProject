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
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [Authorize(Roles = Role.Employer)]
        [HttpGet("Send/{password}/{from}/{to}/{message}")]
        public IActionResult SendMessage(string password,string from, string to,string message)
        {
            var userFrom = _context.User.Where(x => x.Username == from && x.Password == password).Single();
            var userTo = _context.User.Where(x => x.Username == to).Single();
            var message_model = new MessageDTO
            {
                FromUsername = userFrom.Username,
                ToUsername = userTo.Username,
                Message = message
            };

            return Ok(message_model);
        }
    }
}
