var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
FlightWatcher.Application.StartUp.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddJWTAuthentication(builder.Configuration);   

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddHostedService<FlightStatusPollerServices>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
