using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkTransform : MonoBehaviour
{

    private Vector3 oldPosition;

    private NetworkIdentity identity;
    private PlayerData data;

    private float stillUpdateDelay = 0;

    void Start()
    {
        identity = GetComponent<NetworkIdentity>();
        oldPosition = transform.position;
        data = new PlayerData();
        data.position = new Position();
        data.position.x = transform.position.ToString();
        data.position.y = transform.position.ToString();
        data.position.z = transform.position.ToString();

        if (!identity.HasControl)
        {
            enabled = false;
        }
    }

    void Update()
    {
        if (identity.HasControl)
        {
            if (oldPosition != transform.position)
            {
                oldPosition = transform.position;
                stillUpdateDelay = 0;
                SendData();
            }
            else
            {
                stillUpdateDelay += Time.deltaTime;

                if (stillUpdateDelay >= 1)
                {
                    stillUpdateDelay = 0;
                    SendData();
                }
            }
        }
    }

    private void SendData()
    {
        data.position.x = (Mathf.Round(transform.position.x * 1000.0f) / 1000.0f).ToString();
        data.position.y = (Mathf.Round(transform.position.y * 1000.0f) / 1000.0f).ToString();
        data.position.z = (Mathf.Round(transform.position.z * 1000.0f) / 1000.0f).ToString();

        data.id = identity.ID;
        identity.Socket.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(data)));
    }

    private void SendTeamData()
    {
        identity.Socket.Emit("joinTeam", new JSONObject(JsonUtility.ToJson(data)));
    }

    public void assignTeam(string _team)
    {
        data.team = _team;
        SendTeamData();
    }
}
