using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_RESTProject.Models
{
    public class CandidatJob
    {
        public int Id { get; set; }
        public int CandidatId { get; set; }
        public int JobId { get; set; }
    }
}
