using System.Net.WebSockets;

namespace GammaRaySignaling;

public class Client
{
    public string Id = "";
    public string Name = "";
    public string Role = "";
    public string Platform = "";
    public string RoomId = "";
    public long UpdateTimestamp = 0;
    private WebSocket? _webSocket = null;

    public void SetWebSocket(WebSocket? socket)
    {
        _webSocket = socket;
    }
    
    public async void Notify(string msg)
    {
        if (_webSocket == null)
        {
            return;
        }
        var buffer = new ArraySegment<byte>(new byte[msg.Length]);
        await _webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
    }

    public void OnHeartBeat(SignalMessage.SigHeartBeatMessage msg)
    {
        this.UpdateTimestamp = Common.GetCurrentTimestamp();
        if (this.Id != msg.ClientId && msg.ClientId.Length > 0)
        {
            this.Id = msg.ClientId;
        }

        var backMsg = SignalMessage.MakeOnHeartBeatMessage(msg);
        this.SendMessage(backMsg);
    }

    public bool IsOnline()
    {
        var diff = Common.GetCurrentTimestamp() - UpdateTimestamp;
        return diff < 10000;
    }

    public void SendMessage(string msg)
    {
        this.Notify(msg);
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