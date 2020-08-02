﻿using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;

namespace Sheaft.Mappers
{
    public class DeliveryModeProfile : Profile
    {
        public DeliveryModeProfile()
        {
            CreateMap<DeliveryMode, DeliveryModeDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<DeliveryMode, AgreementDeliveryModeDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer));

            CreateMap<CreateDeliveryModeInput, CreateDeliveryModeCommand>();
            CreateMap<UpdateDeliveryModeInput, UpdateDeliveryModeCommand>();
            CreateMap<IdInput, DeleteDeliveryModeCommand>();
        }
    }
}
