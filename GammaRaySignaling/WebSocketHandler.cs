using System.Text;
using Serilog;

namespace GammaRaySignaling.Websocket;

using System.Net.WebSockets;

public class WebSocketHandler
{
    private WebSocket? _webSocket = null;
    private AppContext _appContext;
    private readonly SignalProcessor _processor;

    public WebSocketHandler(AppContext ctx)
    {
        _appContext = ctx;
        _processor = new SignalProcessor(ctx, this);
    }
    
    public async Task Handle(WebSocket ws)
    {
        _webSocket = ws;
        while (_webSocket.State == WebSocketState.Open)
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text && buffer.Array != null)
            {
                var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                if (!_processor.ParseMessage(message))
                {
                    Log.Error("Parse failed");
                }

            } else if (result.MessageType == WebSocketMessageType.Binary)
            {
                
            } else if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine("Close ws");
            }
        }
        Console.WriteLine("End the ws socket");
    }

    public async void SendMessage(string msg)
    {
        if (_webSocket == null)
        {
            Log.Error("Don't have websocket!");
            return;
        }
        var byteArray = Encoding.UTF8.GetBytes(msg);
        var byteSegment = new ArraySegment<byte>(byteArray);
        await _webSocket.SendAsync(byteSegment, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async void Close()
    {
        var task = _webSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
        if (task != null)
        {
            await task;
        }
    }
}