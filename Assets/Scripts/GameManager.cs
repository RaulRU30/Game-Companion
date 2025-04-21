using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ClientDiscovery _discovery;
    private SocketClient _socket;
    
    [SerializeField] private RectTransform playerIcon;

    private void Awake()
    {
        _discovery = GetComponent<ClientDiscovery>();
        _socket = GetComponent<SocketClient>();

        if (_discovery == null || _socket == null) {
            Debug.LogError("Missing required components: ClientDiscovery and/or SocketClient");
            return;
        }

        _discovery.OnServerFound += (ip) => {
            Debug.Log("Server found, connecting to: " + ip);
            _socket.ConnectToServer(ip);
        };
        
        _socket.OnMessageReceived += HandleServerMessage;
    }
    
    private void HandleServerMessage(string json) {
        
        NetworkMessage message = JsonUtility.FromJson<NetworkMessage>(json);
        
        switch (message.type) {
            case "position":
                Vector3 pos = new Vector3(message.payload.x, message.payload.y, message.payload.z);
                Debug.Log("Player position received: " + pos);
                break;

            case "event":
                Debug.Log($"Event: {message.payload.name} in room {message.payload.room}");
                break;

            default:
                Debug.LogWarning("Unknown message type: " + message.type);
                break;
        }
    }
    
    private void UpdatePlayerIcon(Payload payload) {
        Vector2 position2D = new Vector2(payload.x, payload.z); 
        playerIcon.anchoredPosition = position2D * 20f;

        float rotationY = payload.rotationY;
        playerIcon.rotation = Quaternion.Euler(0, 0, -rotationY);

        Debug.Log($"Icon updated: Pos({position2D}), Rot({rotationY}°)");
    }

    
    public void SendTrapCommand(string trapId) {
        var msg = new NetworkMessage {
            type = "command",
            payload = new Payload {
                action = "activate_trap",
                target = trapId
            }
        };

        _socket.SendNetworkMessage(msg);
    }

    
    void Start()
    {
        _discovery.StartDiscovery();
    }
    
    public void SendOpenDoorCommand() {
        _socket.SendMessageToServer("open_door");
    }
    
    public void SendCustomCommand(string command) {
        _socket.SendMessageToServer(command);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
