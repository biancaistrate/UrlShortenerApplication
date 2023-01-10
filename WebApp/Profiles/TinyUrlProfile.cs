using AutoMapper;
using Domain.Master;
using WebApp.Models;

namespace WebApp.Profiles
{
    public class TinyUrlProfile : Profile
    {
        public TinyUrlProfile()
        {
            CreateMap<TinyUrl, TinyUrlDTO>();
            CreateMap<TinyUrlDTO, TinyUrl>();
        }
    }
}
