﻿using System.Text;
using Serilog;

namespace GammaRaySignaling.Websocket;

using System.Net.WebSockets;

public class WebSocketHandler
{
    private WebSocket? _webSocket = null;
    private AppContext _appContext;
    private readonly SignalProcessor _processor;
    private Client? _client = null;
    
    public WebSocketHandler(AppContext ctx)
    {
        _appContext = ctx;
        _processor = new SignalProcessor(ctx, this);
    }
    
    public async Task Handle(WebSocket ws)
    {
        _webSocket = ws;
        try
        {
            var sb = new StringBuilder();
            while (_webSocket.State == WebSocketState.Open)
            {
                sb.Clear();
                while (true)
                {
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Binary && buffer.Array != null)
                    {
                        var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                        sb.Append(message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("Close ws");
                    }

                    if (result.EndOfMessage)
                    {
                        var message = sb.ToString();
                        Log.Information("type: " + result.MessageType + ", count: " + message.Length);
                        if (!_processor.ParseMessage(message, this))
                        {
                            Log.Error("Parse failed");
                        }
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.Warning("Client closed: " + e.Message + ", ");
        }

        var msg = "End of websocket loop, client id: " + (_client != null ? _client.Id : "");
        Log.Information(msg);
        Console.Write(msg);
        if (_client?.Id != null)
        {
            _appContext.GetClientManager().RemoveClient(_client.Id);
        }
    }

    public async void SendMessage(string msg)
    {
        if (_webSocket == null || _webSocket.State != WebSocketState.Open)
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
        if (_webSocket == null || _webSocket.State != WebSocketState.Open)
        {
            return;
        }
        var task = _webSocket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
        if (task != null)
        {
            await task;
        }
    }

    public void AssociateWith(Client c)
    {
        _client = c;
    }
}