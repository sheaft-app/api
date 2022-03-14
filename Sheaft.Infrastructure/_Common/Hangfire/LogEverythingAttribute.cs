using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;

namespace Sheaft.Infrastructure;

public class LogEverythingAttribute : JobFilterAttribute,
    IServerFilter, IElectStateFilter
{
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    public void OnPerforming(PerformingContext context)
    {
    }

    public void OnPerformed(PerformedContext context)
    {
    }

    public void OnStateElection(ElectStateContext context)
    {
        var failedState = context.CandidateState as FailedState;
        if (failedState != null)
        {
        }

        var succeededState = context.CandidateState as SucceededState;
        if (succeededState != null)
        {
        }
    }
}