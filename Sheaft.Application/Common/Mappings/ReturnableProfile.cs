﻿using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Returnable.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class ReturnableProfile : Profile
    {
        public ReturnableProfile()
        {
            CreateMap<Domain.Returnable, ReturnableDto>();
            CreateMap<Domain.Returnable, ReturnableViewModel>();

            CreateMap<CreateReturnableInput, CreateReturnableCommand>();
            CreateMap<UpdateReturnableInput, UpdateReturnableCommand>();
            CreateMap<IdInput, DeleteReturnableCommand>();
        }
    }
}
