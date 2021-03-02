﻿using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RankInformationType : ObjectType<RankInformationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RankInformationDto> descriptor)
        {
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.NextRank);
            descriptor.Field(c => c.PointsToLevelUp);

            descriptor.Field(c => c.Rank)
                .Type<NonNullType<StringType>>();
        }
    }
}