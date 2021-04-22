using System.ComponentModel.DataAnnotations;

namespace BackEnd_RESTProject.Models
{
    public class ForgotPassword
    {
        [Required]
        public string Username { get; set; }
    }
}