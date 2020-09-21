using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Core.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogCommand<T>(this ILogger logger, Result<T> command)
        {
            var args = command.Params?.ToList() ?? new List<object>();
            args.Add(command.Data);
            var datas = args.ToArray();

            if (!command.Success)
                logger.LogError(command.Exception?.Message, command.Exception, datas);
            else
                logger.LogInformation(command.Message, datas);
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
