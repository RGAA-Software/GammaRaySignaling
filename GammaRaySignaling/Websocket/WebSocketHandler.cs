using System.Text;

namespace GammaRaySignaling.Websocket;

using System.Net.WebSockets;

public class WebSocketHandler(AppContext ctx)
{
    private WebSocket? _webSocket = null;
    private AppContext _appContext = ctx;

    public async Task Handle(WebSocket ws)
    {
        _webSocket = ws;
        while (_webSocket.State == WebSocketState.Open)
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text && buffer.Array != null)
            {
                var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                Console.WriteLine($"message: {message}");
                await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            } else if (result.MessageType == WebSocketMessageType.Binary)
            {
                
            } else if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine("Close ws");
            }
        }
        Console.WriteLine("End the ws socket");
    }
}