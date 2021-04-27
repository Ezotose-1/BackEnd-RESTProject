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




            // Function that will allow employer to add a new job on the market
        /// <summary>
        /// Employer : Add job on the market.
        /// </summary>
        [Authorize(Roles= Role.Employer)]
        [HttpPost("Employer/AddJob")]
        public IActionResult Add_Job(JobDTO jobDTO)
        {
            int YourId = int.Parse(User.Identity.Name);
            var job = new Job {
                Description = jobDTO.Description,
                EmployerID  = YourId,
                Paid        = jobDTO.Paid,
                Finished    = false,
                PublicOffer = true
            };
            _context.Job.Add(job);
            _context.SaveChanges();
            return Ok(job);
        }



            // Function that will allow employer to purpose job to candidate
        /// <summary>
        /// Employer : Purpose a new job to a candidat.
        /// </summary>
        [Authorize(Roles= Role.Employer)]
        [HttpPost("Employer/GiveOffer/{OfferId}/{CandidatId}")]
        public IActionResult Give_Job(int OfferId, int CandidatId)
        {
            var offer = _context.Job.ToList().Find(j => j.Id == OfferId);
            if (offer == null)
                return NotFound();
            var candidat = _context.User.ToList().Find(u => u.Id == CandidatId);
            if (candidat == null)
                return NotFound();
            offer.PublicOffer = false;
            offer.CandidatID = CandidatId;
            _context.Job.Update(offer);
            _context.SaveChanges();
            return Ok(offer);
        }


            // Function for candidat to see if they have job offer
        /// <summary>
        /// Employer : List all your job offer
        /// </summary>
        [Authorize(Roles= Role.Employer)]
        [HttpGet("Employer/GetYourOffer")]
        public IActionResult GetYourOffer()
        {
            int YourId = int.Parse(User.Identity.Name);
            var jobs = _context.Job.ToList().Where(x => x.EmployerID == YourId);


            var jobsOffer = new List<EmployerJobDTO>();
            foreach (var j in jobs)
            {
                var candidats = new List<UserModel>();
                if (j.PublicOffer == true){
                    var c = _context.CandidatJob.ToList().Where(x => x.JobId == j.Id);
                    foreach (var u in c)
                    {
                        var user = _context.User.ToList().Find(us => us.Id == u.Id);
                        var model = _mapper.Map<UserModel>(user);
                        candidats.Add(model);
                    }
                }
                jobsOffer.Add(new EmployerJobDTO{
                    Id = j.Id,
                    Description = j.Description,
                    CandidatID = j.CandidatID,
                    Paid = j.Paid,
                    Finished = j.Finished,
                    Accepted = j.Accepted,
                    PublicOffer = j.PublicOffer,
                    Candidats = candidats 
                });
            }
            return Ok(jobsOffer);
        }



            // Function for candidat to see all job offer on the market
        /// <summary>
        /// Candidate : See all job available on the market
        /// </summary>
        [HttpGet("Candidat/GetMarketJobs")]
        public IActionResult GetMarketJobs()
        {
            var jobs = _context.Job.ToList().Where(x => x.PublicOffer == true);
            return Ok(jobs);
        }


            // Function for candidat to see if they have job offer
        /// <summary>
        /// Candidate : See if you have a new job offer
        /// </summary>
        [HttpGet("Candidat/GetJobs")]
        public IActionResult GetJobs()
        {
            int YourId = int.Parse(User.Identity.Name);
            if (_context.User.ToList().Find(x => x.Id == YourId) == null)
                return NotFound();
            var jobs = _context.Job.ToList().Where(x => x.CandidatID == YourId);
            return Ok(jobs);
        }



            // Function to candidate for a job
        /// <summary>
        /// Candidate : Candidate for a public job
        /// </summary>
        [HttpPut("Candidat/{OfferId}")]
        public IActionResult AcceptOffer(int OfferId)
        {
            var offer = _context.Job.ToList().Find(x => x.Id == OfferId);
            if (offer == null || !offer.PublicOffer)
                return NotFound();
            _context.CandidatJob.Add(new CandidatJob {
                CandidatId      = int.Parse(User.Identity.Name),
                JobId           = OfferId
            });
            _context.SaveChanges();
            return Ok();
        }



            // Function for candidat to accept any job offer
        /// <summary>
        /// Candidate : Accept an offer
        /// </summary>
        [HttpPut("Candidat/{OfferId}/{Accept}")]
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

            // Function for candidat to set job to finish
        /// <summary>
        /// Candidate : Finish a job
        /// </summary>
        [HttpPut("Candidat/Finish/{OfferId}/{IsFinish}")]
        public IActionResult FinishOffer(int OfferId, bool IsFinish)
        {
            int YourId = int.Parse(User.Identity.Name);
            var offer = _context.Job.ToList().Find(x => x.Id == OfferId);
            if (offer == null)
                return NotFound();
            if (offer.CandidatID != YourId)
                return NotFound();
            offer.Finished = IsFinish;
            _context.Job.Update(offer);
            _context.SaveChanges();
            return Ok();
        }


            // Function for user to rate other user on a job (candidat and employer)
        /// <summary>
        /// User : Rate your Employer/Candidat on a job
        /// </summary>
        [HttpPut("Rating/{JobId}/{comment}/{gradeOn10}")]
        public IActionResult RatingAJob(int JobId, string comment, int gradeOn10)
        {
            int fromId = int.Parse(User.Identity.Name);

            var FromRates = _context.Rate.ToList().Find(x => x.User_FromId == fromId && x.Job_id == JobId);
            if (FromRates != null)
            {
                return Ok("Sorry you have already rate this Job Offer, if you want to modify your rate" +
                    "please use the dedicated feature.");
            }

           
  
            var toIdQ = _context.Job.ToList().Find(x => x.Id == JobId);
            bool isUserEmployer = User.IsInRole("Employer") || User.IsInRole("Admin");

            var newRate = new Rate
            {
                Job_id = JobId,
                User_FromId = fromId,
                IsUserFromEmployer = isUserEmployer,
                User_ToId = (isUserEmployer ? toIdQ.CandidatID : toIdQ.EmployerID),
                Stars = gradeOn10,
                Comment = comment
            };

            _context.Rate.Add(newRate);
            _context.SaveChanges();

            return Ok(newRate);
        }
    }
}
