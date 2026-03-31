namespace FlightWatcher.Consumers
{
    public class FlightReminderConsumer : IConsumer<FlightReminder>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public FlightReminderConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Consume(ConsumeContext<FlightReminder> context)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
            await notificationService.CreateNotificationAsync(
               context.Message.UserId,
               context.Message.FlightIata,
               null,
               $"Reminder: your flight {context.Message.FlightIata} departs at {context.Message.DepartureTime:HH:mm}"
           );

            await reminderService.MarkAsSentAsync(context.Message.ReminderId);
        }
    }
}
