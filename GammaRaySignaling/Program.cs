using System.Net;
using GammaRaySignaling;using GammaRaySignaling.Controllers;
using GammaRaySignaling.Websocket;
using AppContext = GammaRaySignaling.AppContext;
using Serilog;

// 初始化日志的共享实例
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.File("logs/app.log", 
        rollingInterval: RollingInterval.Day,
        shared: true
    ).CreateLogger();
Log.Information("Info");

var builder = WebApplication.CreateBuilder(args);

// app context
var appContext = new AppContext();
appContext.Init();

// system monitor
var systemMonitor = new SystemMonitor(appContext);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        listenOptions.UseHttps("./Cert/syxmsg.xyz.pfx", "jt182l0laf75v1");
    });
});

var app = builder.Build();

// http handler
var httpHandler = new HttpHandler(appContext, app);
httpHandler.RegisterHandlers();

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