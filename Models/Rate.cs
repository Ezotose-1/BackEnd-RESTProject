using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_RESTProject.Models
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }
        public int Job_id { get; set; }
        public int User_FromId { get; set; }
        public int User_ToId { get; set; }
        public bool IsUserFromEmployer { get; set; }
        public string Comment { get; set; }
        public int Stars { get; set; }
    }
}
