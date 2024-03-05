namespace Scheduler
{
    public abstract class ScheduledJob
    {
        /// <summary>
        /// Gets or sets the date and time of the previous execution of the job.
        /// </summary>
        /// <value>
        /// The date and time of the previous execution of the job.
        /// </value>
        /// <remarks>
        /// This property is automatically set by the <see cref="SchedulerService"/> when the job is executed.
        /// DateTime is always in UTC.
        /// </remarks>
        public DateTime? PreviousExecutionDateTime { get; internal set; }

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Gets the cancellation token that can be used to determine if the job has been cancelled.
        /// </summary>
        /// <value>
        /// The cancellation token that can be used to determine if the job has been cancelled.
        /// </value>
        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        /// <summary>
        /// This method should return the next execution schedule for the job.
        /// </summary>
        /// <returns>
        /// The next execution schedule for the job.
        /// </returns>
        /// <remarks>
        /// You can use the <see cref="PreviousExecutionDateTime"/> property along with parameters passed through constructor to calculate the next execution schedule.
        /// DateTime is always in UTC.
        /// </remarks>
        public abstract DateTime GetNextExecutionSchedule();

        /// <summary>
        /// This method should contain the logic to be executed by the job.
        /// </summary>
        /// <param name="serviceProvider">The service provider to be used to resolve services required by the job.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// You can use the <see cref="CancellationToken"/> property to check if the job has been cancelled.
        /// </remarks>
        public abstract Task Execute(IServiceProvider serviceProvider);

        /// <summary>
        /// Cancels the job.
        /// </summary>
        /// <remarks>
        /// This method sets the <see cref="CancellationToken"/> to cancelled state. Please do not override this method unless you know what you are doing.
        /// </remarks>
        public void Cancel() => _cancellationTokenSource.Cancel();
    }
}
