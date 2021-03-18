﻿using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Agreement.Commands;

namespace Sheaft.Services.Mappings
{
    public class AgreementInputProfile : Profile
    {
        public AgreementInputProfile()
        {
            CreateMap<CreateAgreementDto, CreateAgreementCommand>();
            CreateMap<ResourceIdTimeSlotsDto, AcceptAgreementCommand>()
                .ForMember(c => c.AgreementId, opt => opt.MapFrom(r => r.Id));
            CreateMap<ResourceIdsWithReasonDto, CancelAgreementsCommand>()
                .ForMember(c => c.AgreementIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsWithReasonDto, RefuseAgreementsCommand>()
                .ForMember(c => c.AgreementIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
