using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_RESTProject.DTO
{
    public class MessageDTO
    {
        public string FromEmail { get; set; }
        public List<string> ToEmails { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
