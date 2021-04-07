namespace BackEnd_RESTProject.DTO
{
    public class UpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string Skillset { get; set; }
        public bool Avaible { get; set; }
    }
}