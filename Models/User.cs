namespace BackEnd_RESTProject.Models
{
    
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Skillset { get; set; }
        public bool Avaible { get; set; }
        public string Role { get; set; }
        public bool Advertise { get; set; }
        public string key { get; set; }
        public bool IsBanned { get; set;}

    }
}