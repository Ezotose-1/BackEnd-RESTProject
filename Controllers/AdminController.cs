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
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;

        public AdminController(
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
        /// Admin : Ban a user
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("Ban")]
        public async Task<ActionResult> BanUser(int userId)
        {
            if (_userService.isUserIdValid(userId))
            {
                User user = _userService.GetById(userId);
                user.IsBanned = true;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Successfully banned user " + userId });
            }
            return BadRequest(new { message = "The user id is invalid !" });
        }


        /// <summary>
        /// Admin : Hard delete a user
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("Hard_Delete")]
        public async Task<ActionResult<User>> HardDeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
        }

        /// <summary>
        /// Admin : Update a User (Anyone)
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpPut("Update_User")]
        public IActionResult Update_User(User user, int userId)
        {
            var userToUpdate = _context.User.Find(userId);
            if (userToUpdate == null)
            {
                throw new AppException("User not found");
            }


            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(user.Username) && user.Username != userToUpdate.Username)
            {
                // throw error if the new username is already taken
                if (_context.User.Any(x => x.Username == user.Username))
                {
                    throw new AppException("Username " + user.Username + " is already taken");
                }
                else
                {
                    userToUpdate.Username = user.Username;
                }
            }
            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                userToUpdate.FirstName = user.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                userToUpdate.LastName = user.LastName;
            }
            if (!string.IsNullOrWhiteSpace(user.Skillset))
            {
                userToUpdate.Skillset = user.Skillset;
            }
            userToUpdate.Avaible = user.Avaible;
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                //Updating hashed password into Database table
                userToUpdate.PasswordHash = computeHash(user.Password);
                userToUpdate.Password = user.Password;
            }
            //verif role
            if (!string.IsNullOrWhiteSpace(user.Role))
            {
                userToUpdate.Role = user.Role;
            }

            userToUpdate.Advertise = user.Advertise;
            userToUpdate.key = user.key;
            userToUpdate.IsBanned = user.IsBanned;

            _context.User.Update(userToUpdate);
            _context.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Admin : Update a job
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpPut("Update_Job")]
        public IActionResult Update_Job(Job job, int jobId)
        {
            var jobToUpdate = _context.Job.Find(jobId);
            if (jobToUpdate == null)
            {
                throw new AppException("Job not found");
            }

            //jobToUpdate.Id = job.Id;
            jobToUpdate.Description = job.Description;
            jobToUpdate.CandidatID = job.CandidatID;
            jobToUpdate.EmployerID = job.EmployerID;
            jobToUpdate.Paid = job.Paid;
            jobToUpdate.Finished = job.Finished;
            jobToUpdate.Accepted = job.Accepted;
            jobToUpdate.PublicOffer = job.PublicOffer;
            
            _context.Job.Update(jobToUpdate);
            _context.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Admin : Remove a job
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("Remove_Job")]
        public async Task<ActionResult<Job>> Remove_Job(int id)
        {
            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            else
            {
                _context.Job.Remove(job);
                await _context.SaveChangesAsync();
                return job;
            }
        }

        private static string computeHash(string Password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var input = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hashstring = "";
            foreach (var hashbyte in input)
            {
                hashstring += hashbyte.ToString("x2");
            }
            return hashstring;
        }
    }
}