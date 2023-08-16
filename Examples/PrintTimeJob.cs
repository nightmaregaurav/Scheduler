namespace Scheduler.Examples
{
    public class PrintTimeJob : ScheduledJob
    {
        private int Second { get; }
        private int _count = 1;
        private readonly string _name;

        public PrintTimeJob(string name, int second)
        {
            if (second <= 0) throw new ArgumentException($"{nameof(second)} cannot be less than 1.");
            _name = string.IsNullOrWhiteSpace(name) ? "-" : name;
            Second = second;
        }

        public override DateTime GetNextExecutionSchedule()
        {
            var prev = PreviousExecutionDateTime ?? DateTime.UtcNow;
            return prev.AddSeconds(Second);
        }

        public override Task Execute(IServiceProvider serviceProvider)
        {
            Console.WriteLine($"Executed {_name} {_count} times in interval of {Second} Second{(Second > 1 ? "s" : "")}.");
            _count++;
            return Task.CompletedTask;
        }
    }
}
