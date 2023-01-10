using AutoMapper;
using Domain.Master;
using WebApi.Models;

namespace WebApi.Profiles
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
