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

namespace BackEnd_RESTProject.Controllers
{
    public class MessageController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;

        public MessageController(
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
        [HttpGet("Send/{password}/{from}/{to}/{message}")]
        public IActionResult SendMessage(string password, string from, string to, string message)
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
