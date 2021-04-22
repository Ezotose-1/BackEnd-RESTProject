using System;
using System.ComponentModel.DataAnnotations;

namespace BackEnd_RESTProject.DTO
{
    public class ResetPasswordModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string key { get; set; }

        [Required]
        public string newpassword { get; set; }
    }
}