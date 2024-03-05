using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Scheduler
{
    public class SchedulerService(IServiceProvider serviceProvider) : BackgroundService
    {
        private static readonly Queue<ScheduledJob> PendingJobs = new();
        private static readonly object PendingJobsLock = new();

        /// <summary>
        /// Schedules a job to be executed asynchronously.
        /// </summary>
        /// <typeparam name="TJob">The type of the job to be scheduled.</typeparam>
        /// <param name="job">The job to be scheduled.</param>
        /// <remarks>
        /// Dynamically scheduled jobs will neither resume nor persist when the application restarts.
        /// </remarks>
        public static void ScheduleJobAsync<TJob>(TJob job) where TJob : ScheduledJob
        {
            lock (PendingJobsLock) PendingJobs.Enqueue(job);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            await Execute(stoppingToken);
        }

        private async Task Execute(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;

            while (!stoppingToken.IsCancellationRequested)
            {
                var noPendingTasks = false;
                ScheduledJob? job = null;

                lock (PendingJobsLock)
                {
                    PendingJobs.TryDequeue(out var pendingJob);
                    if (pendingJob != null) job = pendingJob;
                    else noPendingTasks = true;
                }

                if (noPendingTasks || job == null) await Task.Delay(1000, stoppingToken);
                else _ = ScheduleJob(scopedServiceProvider, job);
            }
        }

        private static async Task ScheduleJob(IServiceProvider serviceProvider, ScheduledJob job)
        {
            while (!job.CancellationToken.IsCancellationRequested)
            {
                var nextExecutionTime = job.GetNextExecutionSchedule();
                var universalNextExecutionTime = nextExecutionTime.ToUniversalTime();
                var now = DateTime.UtcNow;
                var nextTick = universalNextExecutionTime < now ? TimeSpan.Zero : universalNextExecutionTime - now;

                await Task.Delay(nextTick, job.CancellationToken);
                if (job.CancellationToken.IsCancellationRequested) continue;

                await job.Execute(serviceProvider);
                job.PreviousExecutionDateTime = nextExecutionTime;
            }
        }
    }

    public static class PrepareSchedulerExtension
    {
        public static void StartScheduler(this IServiceCollection services) => services.AddHostedService<SchedulerService>();
    }
}
