namespace BackEnd_RESTProject.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int CandidatID { get; set; }
        public int EmployerID { get; set; }
        public int Paid { get; set; }
        public bool Finished { get; set; }
    }
}