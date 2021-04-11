using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_RESTProject.DTO
{
    public class JobDTO
    {
        public string Description { get; set; }
        public int CandidatID { get; set; }
        public int EmployerID { get; set; }
        public int Paid { get; set; }
    }
}
