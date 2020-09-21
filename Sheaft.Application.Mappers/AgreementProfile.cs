﻿using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class AgreementProfile : Profile
    {
        public AgreementProfile()
        {
            CreateMap<Agreement, AgreementDto>()
                   .ForMember(c => c.Store, opt => opt.MapFrom(o => o.Store))
                   .ForMember(c => c.Delivery, opt => opt.MapFrom(o => o.Delivery))
                   .ForMember(c => c.SelectedHours, opt => opt.MapFrom(o => o.SelectedHours));

            CreateMap<Agreement, AgreementViewModel>()
                   .ForMember(c => c.Store, opt => opt.MapFrom(o => o.Store))
                   .ForMember(c => c.Delivery, opt => opt.MapFrom(o => o.Delivery))
                   .ForMember(c => c.SelectedHours, opt => opt.MapFrom(o => o.SelectedHours));

            CreateMap<CreateAgreementInput, CreateAgreementCommand>();
            CreateMap<IdTimeSlotGroupInput, AcceptAgreementCommand>();
            CreateMap<IdsWithReasonInput, CancelAgreementsCommand>();
            CreateMap<IdsWithReasonInput, RefuseAgreementsCommand>();
        }
    }
}