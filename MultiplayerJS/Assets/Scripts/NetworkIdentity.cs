using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkIdentity : MonoBehaviour
{

    [SerializeField] private string id;
    [SerializeField] private bool hasControl;

    private SocketIOComponent socket;

    public string ID { get { return id; } }
    public bool HasControl { get { return hasControl; } }
    public SocketIOComponent Socket { get { return socket; } }


    void Awake()
    {
        hasControl = false;
    }

    public void SetControllerID(string _id)
    {
        if (Server.clientID == _id)
        {
            Debug.Log("sup");
            hasControl = true;
        }

        id = _id;
    }

    public void SetSocketReference(SocketIOComponent _socket)
    {
        socket = _socket;
    }
}
