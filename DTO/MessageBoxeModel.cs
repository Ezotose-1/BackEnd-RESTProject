using System;

namespace BackEnd_RESTProject.DTO
{
    
    public class MessageBoxeModel
    { 
        public DateTime When { get; set; }
        public string User_ToName { get; set; }
        public string from_who { get; set; }
        public string subject { get; set; }
       
    }
}