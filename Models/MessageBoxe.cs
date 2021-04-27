using System;

namespace BackEnd_RESTProject.Models
{
    
    public class MessageBoxe
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public string User_ToName { get; set; }
        public string from_who { get; set; }
        public string subject { get; set; }
    }
}