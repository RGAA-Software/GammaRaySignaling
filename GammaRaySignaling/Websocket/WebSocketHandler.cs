using System.Text;

namespace GammaRaySignaling.Websocket;

using System.Net.WebSockets;

public class WebSocketHandler
{
    private WebSocket webSocket;

    public async Task Handle(WebSocket ws)
    {
        webSocket = ws;
        while (webSocket.State == WebSocketState.Open)
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                Console.WriteLine($"message: {message}");
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

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