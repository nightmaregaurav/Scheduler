// using Scheduler;
//
// var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.StartScheduler();
//
// var app = builder.Build();
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
// var printTimeJob = new PrintTimeJob("Static scheduled job that restarts along with system.", 5);
// SchedulerService.ScheduleJobAsync(printTimeJob);
//
// app.MapGet("/print-time", () =>
// {
//     var myJob = new PrintTimeJob("Dynamic scheduled job that does not restarts along with system.", 10);
//     SchedulerService.ScheduleJobAsync(myJob);
//     return "Started";
// }).WithOpenApi();
//
// app.MapPost("/send-message", (string to, string message, int secondsDelay) =>
// {
//     var messageSenderJob = new MessageSenderJob(to, message, DateTime.UtcNow.AddSeconds(secondsDelay));
//     SchedulerService.ScheduleJobAsync(messageSenderJob);
//     return "Scheduled";
// }).WithOpenApi();
//
// app.Run();
