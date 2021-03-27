﻿using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IBusinessClosingQueries
    {
        IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser);
        IQueryable<ClosingDto> GetClosings(RequestUser currentUser);
    }
}