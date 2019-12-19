using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Server : SocketIOComponent
{

    static private Server instance = null;
    static public Server Instance { get { return instance; } }

    public static string clientID { get; private set;}
    private Dictionary<string, NetworkIdentity> serverObjects;
    private Dictionary<string, NetworkIdentity> spawnedObjects;
    private List<Player> playerCharacters;
    public Dictionary<string, NetworkIdentity> ConnectedPlayers { get { return serverObjects; } }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject ballPrefab;

    private int playerAmount = 0;

    public override void Start()
    {
        base.Start();
        serverObjects = new Dictionary<string, NetworkIdentity>();
        playerCharacters = new List<Player>();
        HookEvents();

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    private void HookEvents()
    {
        On("open", (e) => 
        {
            Debug.Log("[Client] - Connected");
        });

        On("register", (e) => 
        {
            clientID = e.data["id"].ToString().Replace("\"", "");
        });

        On("spawn", (e) => 
        {
            string id = e.data["id"].ToString().Replace("\"", "");
            GameObject newPlayer = Instantiate(playerPrefab);
            newPlayer.name = "Player " + id;
            NetworkIdentity ni = newPlayer.GetComponent<NetworkIdentity>();
            ni.SetControllerID(id);
            ni.SetSocketReference(this);
            serverObjects.Add(id, ni);
            playerCharacters.Add(newPlayer.GetComponent<Player>());
            playerAmount++;

            if (playerAmount >= 2)
            {
                for (int i = 0; i < playerCharacters.Count; i++)
                {
                    playerCharacters[i].CanPick = true;
                }
            }
        });

        On("updatePosition", (e) => 
        {
            string id = e.data["id"].ToString().Replace("\"", "");
            string x = e.data["position"]["x"].str;
            string y = e.data["position"]["y"].str;
            string z = e.data["position"]["z"].str;

            float posX = float.Parse(x);
            float posY = float.Parse(y);
            float posZ = float.Parse(z);

            NetworkIdentity ni = serverObjects[id];
            serverObjects[id].gameObject.transform.position = new Vector3(posX, posY, posZ);
        });

        On("teamJoined", (e) =>
        {
            string id = e.data["id"].ToString().Replace("\"", "");
            NetworkIdentity ni = serverObjects[id];
            serverObjects[id].gameObject.GetComponent<Player>().SetPlayer(e.data["team"].str);
        });

        On("disconnected", (e) => 
        {
            string id = e.data["id"].ToString().Replace("\"", "");
            GameObject objectToDestroy = serverObjects[id].gameObject;
            Destroy(objectToDestroy);
            serverObjects.Remove(id);
        });
    }
}

[System.Serializable]
public class PlayerData
{
    public string id;
    public Position position;
    public string team;
}

[System.Serializable]
public class BallData
{
    public string id;
    public Position position;
    public Position direction;
}

[System.Serializable]
public class Position
{
    public string x;
    public string y;
    public string z;
}

