namespace GammaRaySignaling;

public class Room
{
    public String id;
    public String name;
    public List<Client> clients = new List<Client>();
    private object _clientMutex = new object(); 

    public void AddClient(Client client)
    {
        
    }

    public void RemoveClient(String clientId)
    {
        
    }

    public void VisitClients(Action<Client> callback)
    {
        foreach (Client client in clients)
        {
            callback(client);
        }
    }

    public void NotifyAll(String msg)
    {
        lock (_clientMutex)
        {
            VisitClients(client =>
            {
            
            });
        }
    }

    public void NotifyExcept(String exceptId, String msg)
    {
        lock (_clientMutex)
        {
            VisitClients(client =>
            {
                if (client.id == exceptId)
                {
                    return;
                }
                client.Notify(msg);
            });
        }
    }

    public List<Client> GetClients()
    {
        lock (_clientMutex)
        {
            return [..clients];
        }
    }

    public List<Client> GetClientsExcept(String exceptId)
    {
        var targetClients = new List<Client>();
        lock (_clientMutex)
        {
            VisitClients(client =>
            {
                if (client.id != exceptId)
                {
                    targetClients.Add(client);
                }
            });
        }
        return targetClients;
    }
}