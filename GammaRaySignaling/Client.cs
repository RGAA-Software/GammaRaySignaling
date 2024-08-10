using System.Net.WebSockets;
using GammaRaySignaling.Websocket;

namespace GammaRaySignaling;

public class Client
{
    public string Id = "";
    public string Token = "";
    public string Name = "";
    public string Role = "";
    public string Platform = "";
    public string RoomId = "";
    public long UpdateTimestamp = 0;
    private WebSocketHandler? _wsHandler = null;

    public void SetWebSocket(WebSocketHandler? socket)
    {
        _wsHandler = socket;
    }
    
    public async void Notify(string msg)
    {
        if (_wsHandler == null)
        {
            return;
        }
        _wsHandler.SendMessage(msg);
    }

    public void OnHeartBeat(SignalMessage.SigHeartBeatMessage msg)
    {
        UpdateTimestamp = Common.GetCurrentTimestamp();
        if (Id != msg.ClientId && msg.ClientId.Length > 0)
        {
            Id = msg.ClientId;
        }

        var backMsg = SignalMessage.MakeOnHeartBeatMessage(msg);
        SendMessage(backMsg);
    }

    public bool IsOnline()
    {
        var diff = Common.GetCurrentTimestamp() - UpdateTimestamp;
        return diff < 10000;
    }

    public void SendMessage(string msg)
    {
        Notify(msg);
    }

    public void Close()
    {
        _wsHandler?.Close();
    }
}