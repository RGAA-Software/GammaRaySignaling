namespace GammaRaySignaling;

public class Room
{
    public string Id = "";
    public string Name = "";
    private readonly Dictionary<string, Client> _clients = new Dictionary<string, Client>();
    private readonly object _clientMutex = new object(); 

    public void AddClient(Client client)
    {
        lock (_clientMutex)
        {
            if (_clients.ContainsKey(client.Id))
            {
                // todo: warn it
                client.Close();
                _clients.Remove(client.Id);
            }
            _clients[client.Id] = client;
        }
    }

    public void RemoveClient(string clientId)
    {
        lock (_clientMutex)
        {
            if (_clients.ContainsKey(clientId))
            {
                var client = _clients[clientId];
                client.Close();
                _clients.Remove(clientId);
            }
        }
    }

    public void VisitClients(Action<Client> callback)
    {
        foreach (var pair in _clients)
        {
            callback(pair.Value);
        }
    }

    public void NotifyAll(string msg)
    {
        lock (_clientMutex)
        {
            VisitClients(client =>
            {
                client.Notify(msg);
            });
        }
    }

    public void NotifyExcept(string exceptId, string msg)
    {
        lock (_clientMutex)
        {
            VisitClients(client =>
            {
                if (client.Id != exceptId)
                {
                    client.Notify(msg);   
                }
            });
        }
    }

    public List<Client> GetClients()
    {
        lock (_clientMutex)
        {
            return [.._clients.Values];
        }
    }

    public List<Client> GetClientsExcept(string exceptId)
    {
        var targetClients = new List<Client>();
        lock (_clientMutex)
        {
            VisitClients(client =>
            {
                if (client.Id != exceptId)
                {
                    targetClients.Add(client);
                }
            });
        }
        return targetClients;
    }
}