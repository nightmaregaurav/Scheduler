# Scheduler

Scheduler is a simple job scheduling framework in C# that allows you to easily define and execute scheduled tasks in the background. This project provides a flexible base class for creating scheduled jobs, a background service for managing job execution, a static api to register your background services dynamically, and an extension method for integrating the scheduler into your application.

## Features

- Define custom scheduled jobs by extending the `ScheduledJob` base class.
- Enqueue jobs for scheduling using the `ScheduleJobAsync` method.
- Background service (`SchedulerService`) manages the execution of scheduled jobs.
- Integration with Microsoft.Extensions.DependencyInjection for easy setup.

## Getting Started

1. Install the Scheduler package from [NuGet](https://www.nuget.org/packages/nightmaregaurav.scheduler/):

   ```bash
   dotnet add package nightmaregaurav.scheduler
   ```

2. Create your scheduled job classes by extending the `ScheduledJob` base class. Implement the required methods:

   ```csharp
   public class MyCustomJob : ScheduledJob
   {
       public override DateTime GetNextExecutionSchedule()
       {
           // Implement your logic to determine the next execution time.
       }

       public override Task Execute(IServiceProvider serviceProvider)
       {
           // Implement your job's logic here.
       }
   }
   ```

3. Add the `SchedulerService` as a hosted service in your application using the extension method:

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       // Other service registrations...

       // Add the scheduler service
       services.StartScheduler();
   }
   ```
   or in your `program.cs`

   ```csharp
   // Other service registrations...

   // Add the scheduler service
   builder.Services.StartScheduler();
   ```

4. Enqueue jobs for scheduling:

   ```csharp
   var myJob = new MyCustomJob();
   await SchedulerService.ScheduleJobAsync(myJob);
   ```

5. Run your application, and the scheduler will automatically start executing the scheduled jobs in the background.

## License

Scheduler is released under the MIT License. You can find the full license details in the [LICENSE](LICENSE) file.

Made with ❤️ by [NightmareGaurav](https://github.com/nightmaregaurav).

---
Open For Contribution
---
We welcome contributions from the community! If you find any issues or have suggestions for improvements, feel free to open a pull request or issue. Your contributions help make this project better for everyone.
