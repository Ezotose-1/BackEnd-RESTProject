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
using System.Threading.Tasks;

namespace BackEnd_RESTProject.Controllers
{
    public class MessageController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;
        private readonly IEmailService _emailService;

        public MessageController(
            Context context,
            IUserService userService,
            IMapper mapper,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
            Configuration = configuration;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpGet("mail")]
        public async Task<IActionResult> SendMessage(MessageDTO model)
        {
            var response = await _emailService.SendEmailAsync(model.FromEmail, model.ToEmails, model.Subject, model.Message);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return Ok("Email sent " + response.StatusCode);
            }
            else
            {
                return BadRequest("Email sending failed " + response.StatusCode);
            }
        }
        /*
        [AllowAnonymous]
        [HttpPost("forgotpassword")]
        public IActionResult ForgotPassword(ForgotPassword model)
        {
            return Ok(_userService.ForgotPassword(model.Username));
        }
        */
    }

}
