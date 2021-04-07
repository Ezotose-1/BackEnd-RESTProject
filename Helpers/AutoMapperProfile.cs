using AutoMapper;
using BackEnd_RESTProject.DTO;
using BackEnd_RESTProject.Models;

namespace BackEnd_RESTProject.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}