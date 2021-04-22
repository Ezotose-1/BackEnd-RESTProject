using System.Collections.Generic;

namespace BackEnd_RESTProject.DTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Skillset { get; set; }
        public bool Avaible { get; set; }
        public string Role { get; set; }
        public List<RateDTO> Rates { get; set; }
    }
}