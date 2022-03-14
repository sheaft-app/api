﻿using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace Sheaft.Infrastructure;

public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
{
    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        context.JobExpirationTimeout = TimeSpan.FromDays(3);
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        context.JobExpirationTimeout = TimeSpan.FromDays(3);
    }
}