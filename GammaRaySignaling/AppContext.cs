namespace GammaRaySignaling;

public class AppContext
{
    private ClientManager _clientManager;
    private RoomManager _roomManager;

    public AppContext()
    {
    }

    public void Init()
    {
        _clientManager = new ClientManager();
        _roomManager = new RoomManager();
    }
}