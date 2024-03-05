namespace Scheduler.Examples
{
    public class MessageSenderJobExample(string to, string message, DateTime schedule) : ScheduledJob
    {
        public override DateTime GetNextExecutionSchedule() => schedule;

        public override async Task Execute(IServiceProvider serviceProvider)
        {
            Console.WriteLine($"Sending message to {to} with message {message}.");
            Cancel();
        }
    }
}
