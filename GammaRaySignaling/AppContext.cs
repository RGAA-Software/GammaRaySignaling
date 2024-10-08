﻿namespace GammaRaySignaling;

public class AppContext
{
    private ClientManager _clientManager;
    private RoomManager _roomManager;

    public AppContext()
    {
        _clientManager = new ClientManager(this);
        _roomManager = new RoomManager(this);
    }

    public void Init()
    {
    }

    public ClientManager GetClientManager()
    {
        return _clientManager;
    }

    public RoomManager GetRoomManager()
    {
        return _roomManager;
    }
}