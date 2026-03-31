var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
FlightWatcher.Application.StartUp.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddJWTAuthentication(builder.Configuration);   

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddHostedService<FlightStatusPollerServices>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<FlightReminderConsumer>();
    x.AddQuartzConsumers();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddQuartz(q =>
{
    q.UsePersistentStore(s =>
    {
        s.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
