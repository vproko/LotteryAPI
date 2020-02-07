using AutoMapper;
using Lottery.DomainClasses.Models;
using Lottery.DataModels.Models;
using System;

namespace Lottery.Services.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(uDto => uDto.Password, src => src.Ignore())
                .ReverseMap();

            CreateMap<RegisterModel, User>()
                .ForMember(u => u.UserId, src => src.ResolveUsing(rm => Guid.NewGuid()))
                .ForMember(u => u.Joined, src => src.ResolveUsing(rm => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateModel, User>()
                .ForMember(u => u.Password, src => src.MapFrom(um => um.NewPassword));

            CreateMap<Winner, WinnerDTO>()
                .ReverseMap()
                .ForMember(w => w.WinnerId, src => src.ResolveUsing(WDto => Guid.NewGuid()));

            CreateMap<Session, SessionDTO>()
                .ReverseMap()
                .ForMember(s => s.SessionId, src => src.ResolveUsing(sDto => Guid.NewGuid()))
                .ForMember(s => s.StartDate, src => src.ResolveUsing(sDto => DateTime.UtcNow))
                .ForMember(s => s.EndDate, src => src.ResolveUsing(sDto => DateTime.UtcNow.AddDays(7)));

            CreateMap<Draw, DrawDTO>()
                .ReverseMap()
                .ForMember(d => d.DrawId, src => src.ResolveUsing(dDto => Guid.NewGuid()))
                .ForMember(d => d.Date, src => src.ResolveUsing(dDto => DateTime.UtcNow));

            CreateMap<Ticket, TicketDTO>()
                .ReverseMap()
                .ForMember(t => t.TicketId, src => src.ResolveUsing(tDto => Guid.NewGuid()))
                .ForMember(t => t.CreateDate, src => src.ResolveUsing(tDto => DateTime.UtcNow));

            CreateMap<Prize, PrizeDTO>()
                .ReverseMap()
                .ForMember(p => p.PrizeId, src => src.ResolveUsing(pDto => Guid.NewGuid()));
        }
    }
}
