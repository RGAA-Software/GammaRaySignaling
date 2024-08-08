using GammaRaySignaling;using GammaRaySignaling.Controllers;
using GammaRaySignaling.Websocket;
using AppContext = GammaRaySignaling.AppContext;

var builder = WebApplication.CreateBuilder(args);
var appContext = new AppContext();
appContext.Init();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseWebSockets();

app.MapGet("/ping", () => "pong").WithName("ping");
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/signaling")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var websocket = await context.WebSockets.AcceptWebSocketAsync();
            var handler = new WebSocketHandler(appContext);
            await handler.Handle(websocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.Run();