using Microsoft.OpenApi.Models;
using SignalR.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "SignalR.Server", Version = "v1" });
    swaggerGenOptions.AddSignalRSwaggerGen();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                // .WithOrigins("http://localhost:4200");
                .AllowAnyOrigin();
        });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//NOTE: localhost:5020/hubs/chat
app.MapHub<MessageHub>("/hubs/messages");

app.UseCors("CorsPolicy");

app.Run();