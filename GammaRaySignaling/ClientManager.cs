using System.Timers;
using Serilog;
using Timer = System.Timers.Timer;

namespace GammaRaySignaling;

public class ClientManager
{
    private AppContext _context;
    private readonly Timer _timer;
    private readonly object _clientsMutex = new object();
    private readonly Dictionary<string, Client> _clients = new Dictionary<string, Client>();
    
    public ClientManager(AppContext ctx)
    {
        _context = ctx;

        _timer = new Timer();
        _timer.AutoReset = true;
        _timer.Interval = 2000;
        _timer.Enabled = true;
        _timer.Elapsed += OnTimerOut;
        _timer.Start();
    }

    private void OnTimerOut(object? sender, ElapsedEventArgs e)
    {
        Log.Information("timer out..." + e.SignalTime);
        TidyClientByOnlineStatus();
    }

    public void AddClient(Client client)
    {
        lock (_clientsMutex)
        {
            _clients[client.Id] = client;
        }
    }

    public void RemoveClient(string clientId)
    {
        lock (_clientsMutex)
        {
            if (_clients.ContainsKey(clientId))
            {
                _clients.Remove(clientId);
            }
        }
    }

    public bool IsClientOnline(string clientId)
    {
        return GetOnlineClientById(clientId) != null;
    }

    public Client? GetOnlineClientById(string clientId)
    {
        lock (_clientsMutex)
        {
            if (!_clients.TryGetValue(clientId, out var client))
            {
                return null;
            }
            return client.IsOnline() ? client : null;
        }
    }
    
    private void TidyClientByOnlineStatus()
    {
        lock (_clientsMutex)
        {
            var currentTimestamp = Common.GetCurrentTimestamp();
            var toRemoveIds = new List<string>();
            foreach (var pair in _clients)
            {
                if (!pair.Value.IsOnline())
                {
                    toRemoveIds.Add(pair.Key);
                }
            }
            foreach (var removeId in toRemoveIds)
            {
                _clients.Remove(removeId);
            }
        }
    }

    public List<Client> GetOnlineClients()
    {
        lock (_clientsMutex)
        {
            var onlineClients = new List<Client>();
            foreach (var pair in _clients)
            {
                if (pair.Value.IsOnline())
                {
                    onlineClients.Add(pair.Value);
                }
            }
            return onlineClients;
        }
    }
}