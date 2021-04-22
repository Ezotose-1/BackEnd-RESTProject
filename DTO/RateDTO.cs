using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_RESTProject.DTO
{
    public class RateDTO
    {
        public string JobDescription { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}
