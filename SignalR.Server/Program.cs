using SignalR.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.MapHub<MessageHub>("/hubs/chat");

app.UseCors("CorsPolicy");

app.Run();