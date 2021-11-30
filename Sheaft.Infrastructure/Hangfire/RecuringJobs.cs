using Sheaft.Application.Configurations;

namespace Sheaft.Infrastructure.Hangfire
{
    internal static class RecuringJobs
    {
        public static void Register(RoutineConfiguration configuration)
        {
            // RecurringJob.AddOrUpdate<Dispatcher>("47854516c21a48dc907b429a96cd0edd", mediatr =>
            //     mediatr.Execute(nameof(Command), new Command(new RequestUser("recurring-jobs")), CancellationToken.None), 
            //     "15 1 * * *");
        }
    }
}