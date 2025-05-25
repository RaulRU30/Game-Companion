using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private SocketClient _socket;

    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private string serverIP = "192.168.1.78";

    private void Awake()
    {
        _socket = GetComponent<SocketClient>();

        if (_socket == null)
        {
            Debug.LogError("Missing required components: ClientDiscovery and/or SocketClient");
            return;
        }

        _socket.OnMessageReceived += HandleServerMessage;
    }

    private void HandleServerMessage(string json)
    {

        NetworkMessage message = JsonUtility.FromJson<NetworkMessage>(json);

        switch (message.type)
        {
            case "position":
                UpdatePlayerIcon(message.payload);
                break;

            case "event":
                Debug.Log($"Event: {message.payload.name} in room {message.payload.room}");
                break;

            default:
                Debug.LogWarning("Unknown message type: " + message.type);
                break;
        }
    }

    private void UpdatePlayerIcon(Payload payload)
    {
        Vector2 position2D = new Vector2(payload.x, payload.z);
    Vector2 offset = new Vector2(-334.6f, 296.5f);  // ajustado manualmente
    float scale = 20f;

    playerIcon.anchoredPosition = position2D * scale + offset;

    float rotationY = payload.rotationY;
    playerIcon.rotation = Quaternion.Euler(0, 0, -rotationY);

    Debug.Log($"Icon updated: Pos({position2D}), Rot({rotationY})");
    }


    public void SendTrapCommand(string trapId)
    {
        var msg = new NetworkMessage
        {
            type = "command",
            payload = new Payload
            {
                action = "activate_trap",
                target = trapId
            }
        };

        _socket.SendNetworkMessage(msg);
    }



    public void SendOpenDoorCommand(string doorId)
    {
        Debug.Log("Button pressed");
        var msg = new NetworkMessage
        {
            type = "command",
            payload = new Payload
            {
                action = "open_door",
                target = doorId
            }
        };

        _socket.SendNetworkMessage(msg);
    }

    public void SendCloseDoorCommand(string doorId)
    {
        var msg = new NetworkMessage
        {
            type = "command",
            payload = new Payload
            {
                action = "close_door",
                target = doorId
            }
        };

        _socket.SendNetworkMessage(msg);
    }


    void Start()
    {
    }

    public void SendOpenDoorCommand()
    {
        _socket.SendMessageToServer("open_door");
    }

    public void SendCustomCommand(string command)
    {
        _socket.SendMessageToServer(command);
    }
}
