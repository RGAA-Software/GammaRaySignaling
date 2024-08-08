using System.Net.WebSockets;

namespace GammaRaySignaling;

public class Client
{
    public String id;
    public String name;
    public String role;
    public String platform;
    public String roomId;
    public Int64 updateTimestamp;
    public WebSocket websocket;

    public void Notify(String msg)
    {
        
    }

    public void OnHeartBeat()
    {
        
    }

    public bool IsOnline()
    {
        return false;
    }

    public void SendMessage(String msg)
    {
        
    }

    public void Close()
    {
        
    }
}