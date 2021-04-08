using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_RESTProject.DTO
{
    public class MessageDTO
    {
        public string FromUsername { get; set; }
        public string ToUsername { get; set; }
        public string Message { get; set; }
    }
}
