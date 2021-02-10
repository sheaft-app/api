﻿using System;
using System.Threading.Tasks;

namespace Sheaft.Application.Common.Interfaces.Services
{
    public interface ISignalrService
    {
        Task SendNotificationToGroupAsync<T>(Guid groupId, string eventName, T content);
        Task SendNotificationToUserAsync<T>(Guid userId, string eventName, T content);
    }
}