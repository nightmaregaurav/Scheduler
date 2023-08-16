namespace Scheduler
{
    public abstract class ScheduledJob
    {
        public DateTime? PreviousExecutionDateTime { get; set; }
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public abstract DateTime GetNextExecutionSchedule();
        public abstract Task Execute(IServiceProvider serviceProvider);
        public void Cancel(IServiceProvider serviceProvider) => _cancellationTokenSource.Cancel();
    }
}
