using System.ComponentModel.DataAnnotations;

namespace BackEnd_RESTProject.DTO
{
    
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool isEmployer { get; set; }
    }
}