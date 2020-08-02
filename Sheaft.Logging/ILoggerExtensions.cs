using Microsoft.Extensions.Logging;
using Sheaft.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Logging
{
    public static class ILoggerExtensions
    {
        public static void LogCommand<T>(this ILogger logger, CommandResult<T> command)
        {
            var args = command.Message.Value?.ToList() ?? new List<object>();
            args.Add(command.Result);
            var datas = args.ToArray();

            if (!command.Success)
                logger.LogError(command.Exception.Message, command.Exception);
            else
                logger.LogInformation(command.Message.Key, datas);
        }

        public static void LogInformation(this ILogger logger, Enum e, params object[] args)
        {
            var i = (int)Convert.ChangeType(e, e.GetTypeCode());

            var eventName = Enum.GetName(e.GetType(), e);
            var eventScope = e.GetType().Name;
            var parameters = args.Append(eventScope).Append(eventName).ToArray();

            logger.LogInformation(i, "Event scope: {eventScope}, name: {eventName}", parameters);
        }

        public static void LogInformation<T>(this ILogger<T> logger, Enum e, params object[] args)
        {
            var i = (int)Convert.ChangeType(e, e.GetTypeCode());

            var eventName = Enum.GetName(e.GetType(), e);
            var eventScope = e.GetType().Name;
            var parameters = args.Append(eventScope).Append(eventName).ToArray();

            logger.LogInformation(i, "Event scope: {eventScope}, name: {eventName}", parameters);
        }
    }
}
