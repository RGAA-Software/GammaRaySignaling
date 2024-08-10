using System.Text.Json;
using GammaRaySignaling.Websocket;
using Serilog;

namespace GammaRaySignaling;

public class SignalProcessor
{
    private readonly AppContext _context;
    private readonly ClientManager _clientManager;
    private readonly RoomManager _roomManager;
    private WebSocketHandler _handler;
    private Client _client;
    
    public SignalProcessor(AppContext ctx, WebSocketHandler handler)
    {
        _context = ctx;
        _clientManager = ctx.GetClientManager();
        _roomManager = ctx.GetRoomManager();
        _handler = handler;
        _client = new Client();
        _client.SetWebSocket(handler);
    }
    
    public bool ParseMessage(string message)
    {
        try
        {
            var jsonObject = JsonSerializer.Deserialize<dynamic>(message);
            if (jsonObject == null)
            {
                Log.Error("Null json object: " + message);
                return false;
            }

            var sigName = jsonObject[SignalMessage.KeySigName];
            var token = jsonObject[SignalMessage.KeyToken];
            
            if (sigName == SignalMessage.SigNameHello)
            {
                // hello
                var allowReSend = false;
                if (jsonObject.TryGetProperty(SignalMessage.KeyAllowReSend, out JsonElement ele))
                {
                    allowReSend = ele.GetBoolean();
                }
                _onSigHelloCbk(new SignalMessage.SigHelloMessage
                {
                    SigName = sigName,
                    Token = token,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    Platform = jsonObject[SignalMessage.KeyPlatform],
                    AllowReSend = allowReSend,
                });
                
            } else if (sigName == SignalMessage.SigNameCreateRoom)
            {
                // create room
                _onSigCreateRoomCbk(new SignalMessage.SigCreateRoomMessage
                {
                    SigName = sigName,
                    Token = token,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RemoteClientId = jsonObject[SignalMessage.KeyRemoteClientId],
                });
                
            } else if (sigName == SignalMessage.SigNameJoinRoom)
            {
                // join room
                _onSigJoinRoomCbk(new SignalMessage.SigJoinRoomMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RemoteClientId = jsonObject[SignalMessage.KeyRemoteClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId]
                });
                
            } else if (sigName == SignalMessage.SigNameInviteClient)
            {
                OnSigInviteClientCbk(new SignalMessage.SigInviteClientMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RemoteClientId = jsonObject[SignalMessage.KeyRemoteClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId]
                });
            } 
            else if (sigName == SignalMessage.SigNameLeaveRoom)
            {
                // leave room
                _onSigLeaveRoomCbk(new SignalMessage.SigLeaveRoomMessage
                {
                    SigName = sigName,
                    Token = token,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId],
                });
                
            } else if (sigName == SignalMessage.SigNameHeartBeat)
            {
                // heart beat
                _onSigHeartBeatCbk(new SignalMessage.SigHeartBeatMessage
                {
                    SigName = sigName,
                    Token = token,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    Index = jsonObject[SignalMessage.KeyIndex]
                });
                
            } else if (sigName == SignalMessage.SigNameOfferSdp)
            {
                // offer sdp
                _onSigOfferSdpCbk(new SignalMessage.SigOfferSdpMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId],
                    Sdp = jsonObject[SignalMessage.KeySdp]
                });
                
            } else if (sigName == SignalMessage.SigNameAnswerSdp)
            {
                // answer sdp
                _onSigAnswerSdpCbk(new SignalMessage.SigAnswerSdpMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId],
                    Sdp = jsonObject[SignalMessage.KeySdp]
                });

            } else if (sigName == SignalMessage.SigNameIce)
            {
                // ice
                _onSigIceCbk(new SignalMessage.SigIceMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId],
                    Ice = jsonObject[SignalMessage.KeyIce],
                    Mid = jsonObject[SignalMessage.KeyMid],
                    SdpMLineIndex = jsonObject[SignalMessage.KeySdpMLineIndex]
                });
                
            } else if (sigName == SignalMessage.SigNameForceIFrame)
            {
                // force i frame
                _onSigForceIFrameCbk(new SignalMessage.SigForceIFrameMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId],
                });
                
            } else if (sigName == SignalMessage.SigNameReqControl)
            {
                // request control
                _onSigReqControlCbk(new SignalMessage.SigReqControlMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RemoteClientId = jsonObject[SignalMessage.KeyRemoteClientId],
                });

            } else if (sigName == SignalMessage.SigNameUnderControl)
            {
                // under control
                _onSigUnderControlCbk(new SignalMessage.SigUnderControlMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    SelfClientId = jsonObject[SignalMessage.KeySelfClientId],
                    ControllerId = jsonObject[SignalMessage.KeyControlClientId],
                });
                
            } else if (sigName == SignalMessage.SigNameOnDataChannelReady)
            {
                // data channel ready
                _onSigRemoteDataChannelReadyCbk(new SignalMessage.SigOnRemoteDataChannelReadyMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    SelfClientId = jsonObject[SignalMessage.KeySelfClientId],
                    ControllerId = jsonObject[SignalMessage.KeyControlClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId]
                });

            } else if (sigName == SignalMessage.SigNameOnRejectControl)
            {
                // reject control
                _onRejectControlCbk(new SignalMessage.SigOnRejectControlMessage
                {
                    SigName = sigName,
                    Token = token,
                    OriginMessage = message,
                    ClientId = jsonObject[SignalMessage.KeyClientId],
                    RoomId = jsonObject[SignalMessage.KeyRoomId],
                    ControllerId = jsonObject[SignalMessage.KeyControlClientId]
                });
            }

            return true;
        }
        catch (Exception e)
        {
            Log.Error("Parse json failed: " + e.Message + ", msg: " + message);
            return false;
        }
    }

    private Room? GetRoomById(string roomId, Client reqClient)
    {
        var room = _context.GetRoomManager().FindRoomById(roomId);
        if (room == null)
        {
            reqClient.Notify(Errors.MakeKnownErrorMessage(Errors.ErrNoRoomFound));
            Log.Error("Can't find room: " + roomId + ", req client: " + reqClient.Id);
        }
        return room;
    }

    private static bool IsClientIdOk(string clientId, string fromMethod)
    {
        if (clientId.Length == 9)
        {
            return true;
        }
        Log.Error("Error client id: " + clientId + ", from method: " + fromMethod);
        return false;
    }

    private void _onSigHelloCbk(SignalMessage.SigHelloMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "Hello")) return;
        
        if (_clientManager.IsClientOnline(msg.ClientId) && !msg.AllowReSend)
        {
            _handler.SendMessage(Errors.MakeKnownErrorMessage(Errors.ErrAlreadyLogin));
            return;
        }

        _client.Id = msg.ClientId;
        _client.Token = msg.Token;
        _client.Platform = msg.Platform;
        _client.UpdateTimestamp = Common.GetCurrentTimestamp();
        _clientManager.AddClient(_client);
        _client.Notify(SignalMessage.MakeOnHelloMessage(_client.Token, _client.Id));
    }

    private void _onSigCreateRoomCbk(SignalMessage.SigCreateRoomMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "CreateRoom")) return;

        var room = _roomManager.CreateRoom(msg.ClientId, msg.RemoteClientId);
        _client.Id = msg.ClientId;
        _client.Token = msg.Token;
        _client.Notify(SignalMessage.MakeOnCreatedRoomMessage(msg.Token, msg.ClientId, msg.RemoteClientId, room));
    }
    
    private void _onSigJoinRoomCbk(SignalMessage.SigJoinRoomMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "JoinRoom")) return;
        
        // find room
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        
        // enter room
        room.AddClient(_client);
        
        // telling the client, you are already in room now. 
        _client.Notify(SignalMessage.MakeOnJoinedRoomMessage(msg.Token, room, msg.ClientId, msg.RemoteClientId));
        // telling the other client, new client in.
        room.NotifyExcept(msg.ClientId, SignalMessage.MakeOnRemoteJoinedRoomMessage(msg.Token, room, msg.RemoteClientId));
    }
    
    private void OnSigInviteClientCbk(SignalMessage.SigInviteClientMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "InviteClient")) return;
        if (!IsClientIdOk(msg.RemoteClientId, "InviteClient")) return;
        
        // remote client
        var remoteClient = _clientManager.GetOnlineClientById(msg.RemoteClientId);
        if (remoteClient == null)
        {
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrClientOffline));
            Log.Error("Can't find remote client: " + msg.RemoteClientId);
            return;
        }
        
        // room
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        
        room.AddClient(remoteClient);
        // telling the remote peer, you are invited to room
        remoteClient.Notify(SignalMessage.MakeOnInvitedToRoomMessage(msg.Token, room, msg.ClientId, msg.RemoteClientId));
        // telling the invitor, remote peer is in room now
        _client.Notify(SignalMessage.MakeOnRemoteInvitedToRoomMessage(msg.Token, room, msg.ClientId, msg.RemoteClientId));
    }
    
    private void _onSigLeaveRoomCbk(SignalMessage.SigLeaveRoomMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "LeaveRoom")) return;
        
        // find room
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        
        // remove the client 
        room.RemoveClient(msg.ClientId);
        // telling the client, you left
        _client.Notify(SignalMessage.MakeOnLeftRoomMessage(msg.Token, room, msg.ClientId));
        // telling the other client, someone left
        room.NotifyExcept(msg.ClientId, SignalMessage.MakeOnRemoteClientLeftMessage(msg.Token, room, msg.ClientId));
    }

    private void _onSigHeartBeatCbk(SignalMessage.SigHeartBeatMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "HeartBeat")) return;

        var client = _clientManager.GetOnlineClientById(msg.ClientId);
        if (client == null)
        {
            _client.Id = msg.ClientId;
            _client.Token = msg.Token;
            _client.Platform = msg.Platform;
            _client.UpdateTimestamp = Common.GetCurrentTimestamp();
            _clientManager.AddClient(_client);
        }
        _client.OnHeartBeat(msg);
    }
    
    private void _onSigOfferSdpCbk(SignalMessage.SigOfferSdpMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "OfferSdp")) return;
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        room.NotifyExcept(msg.ClientId, msg.OriginMessage);
    }
    
    private void _onSigAnswerSdpCbk(SignalMessage.SigAnswerSdpMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "AnswerSdp")) return;
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        room.NotifyExcept(msg.ClientId, msg.OriginMessage);
    }
    
    private void _onSigIceCbk(SignalMessage.SigIceMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "Ice")) return;
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        room.NotifyExcept(msg.ClientId, msg.OriginMessage);
    }
    
    private void _onSigForceIFrameCbk(SignalMessage.SigForceIFrameMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "IFrame")) return;
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        room.NotifyExcept(msg.ClientId, msg.OriginMessage);   
    }
    
    private void _onSigReqControlCbk(SignalMessage.SigReqControlMessage msg)
    {
        if (!IsClientIdOk(msg.ClientId, "ReqControl")) return;
        if (!IsClientIdOk(msg.RemoteClientId, "ReqControl")) return;
        var remoteClient = _clientManager.GetOnlineClientById(msg.RemoteClientId);
        if (remoteClient == null)
        {
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrClientOffline));
            Log.Error("[ReqControl]Can't find remote client: " + msg.RemoteClientId);
            return;
        }
        remoteClient.Notify(msg.OriginMessage);
    }

    private void _onSigUnderControlCbk(SignalMessage.SigUnderControlMessage msg)
    {
        if (!IsClientIdOk(msg.ControllerId, "UnderControl")) return;
        var controllerClient = _clientManager.GetOnlineClientById(msg.ControllerId);
        if (controllerClient == null)
        {
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrClientOffline));
            Log.Error("[UnderControl]Can't find remote client: " + msg.ControllerId);
            return;
        }
        controllerClient.Notify(msg.OriginMessage);
    }
    
    private void _onSigRemoteDataChannelReadyCbk(SignalMessage.SigOnRemoteDataChannelReadyMessage msg)
    {
        if (!IsClientIdOk(msg.ControllerId, "DataChannelReady")) return;
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        room.NotifyExcept(msg.SelfClientId, msg.OriginMessage);
    }

    private void _onRejectControlCbk(SignalMessage.SigOnRejectControlMessage msg)
    {
        if (!IsClientIdOk(msg.ControllerId, "RejectControl")) return;
        var room = _roomManager.FindRoomById(msg.RoomId);
        if (room == null)
        {
            Log.Error("Can't find room: " + msg.RoomId);
            _client.Notify(SignalMessage.MakeOnSigKnownErrorMessage(msg.Token, Errors.ErrNoRoomFound));
            return;
        }
        room.NotifyExcept(msg.ClientId, msg.OriginMessage);
    }
}