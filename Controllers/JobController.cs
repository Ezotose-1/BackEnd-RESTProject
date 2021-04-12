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
    public class JobController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;

        public JobController(
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


            // Function that will allow employer to add a new job
        /// <summary>
        /// Employer : Purpose a new job to a candidat.
        /// </summary>
        [Authorize(Roles= Role.Employer)]
        [HttpPost]
        public IActionResult Add_Job(JobDTO jobDTO)
        {
            if (_context.User.ToList().Find(x => x.Id == jobDTO.CandidatID) == null)
                return NotFound();
            if (_context.User.ToList().Find(x => x.Id == jobDTO.EmployerID) == null)
                return NotFound();
            var job = new Job {
                Description = jobDTO.Description,
                CandidatID  = jobDTO.CandidatID,
                EmployerID  = jobDTO.EmployerID,
                Paid        = jobDTO.Paid,
                Finished    = false
            };
            _context.Job.Add(job);
            _context.SaveChanges();
            return Ok(job);
        }


            // Function for candidat to see if they have job offer
        /// <summary>
        /// Employer : List all your job offer
        /// </summary>
        [Authorize(Roles= Role.Employer)]
        [HttpGet("GetYourOffer")]
        public IActionResult GetYourOffer()
        {
            int YourId = int.Parse(User.Identity.Name);
            var jobs = _context.Job.ToList().Where(x => x.EmployerID == YourId);
            if (jobs == null)
                return NotFound();
            return Ok(jobs);
        }



            // Function for candidat to see if they have job offer
        /// <summary>
        /// Candidate : See if you have a new job offer
        /// </summary>
        [HttpGet("GetJobs")]
        public IActionResult GetJobs()
        {
            int YourId = int.Parse(User.Identity.Name);
            if (_context.User.ToList().Find(x => x.Id == YourId) == null)
                return NotFound();
            var jobs = _context.Job.ToList().Where(x => x.CandidatID == YourId);
            return Ok(jobs);
        }


            // Function for candidat to accept any job offer
        /// <summary>
        /// Candidate : Accept an offer
        /// </summary>
        [HttpPut("{OfferId}/{Accept}")]
        public IActionResult AcceptOffer(int OfferId, bool Accept)
        {
            var offer = _context.Job.ToList().Find(x => x.Id == OfferId);
            if (offer == null)
                return NotFound();
            offer.Accepted = Accept;
            _context.Job.Update(offer);
            _context.SaveChanges();
            return Ok();
        }
    }
}