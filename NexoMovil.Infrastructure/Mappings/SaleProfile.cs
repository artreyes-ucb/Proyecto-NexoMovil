using AutoMapper;
using NexoMovil.Core.DTOs;
using NexoMovil.Core.Entities;

namespace NexoMovil.Infrastructure.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleResponseDto>()
                .ForMember(dest => dest.SaleId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date,
                    opt => opt.ConvertUsing<DateTimeToStringConverter, DateTime>(src => src.Date))
                .ForMember(dest => dest.Brand,
                    opt => opt.MapFrom(src => src.Product.Brand))
                .ForMember(dest => dest.Model,
                    opt => opt.MapFrom(src => src.Product.Model))
                .ForMember(dest => dest.UnitPrice,
                    opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.CustomerEmail,
                    opt => opt.MapFrom(src => src.User.Email));
        }
    }
}