using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd_RESTProject.DTO
{
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}