using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackEnd_RESTProject.DTO
{
    
    public class EmployerJobDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int CandidatID { get; set; }
        public int Paid { get; set; }
        public bool Finished { get; set; }
        public bool Accepted { get; set; }
        public bool PublicOffer { get; set; }
        public List<UserModel> Candidats { get; set; }
    }
}