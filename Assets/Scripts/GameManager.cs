using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private SocketClient _socket;
     public TextMeshProUGUI[] codeSlotsText;
     private Color defaultColor = new Color32(0x35, 0x9E, 0xB2, 0xFF); // #359EB2
     public Image[] codeSlotsImage;
    public Image successIndicator;
    private int currentIndex = 0;
    //Vector2 worldMin = new Vector2(-3.9f, -35.72f);
    //Vector2 worldMax = new Vector2(51.89f, 12.04f);

    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private string serverIP = "192.168.1.78";
    [SerializeField] private Vector2 worldMin = new Vector2(-3.9f, -35.72f);
    [SerializeField] private Vector2 worldMax = new Vector2(20f, 12.04f);
    [SerializeField] private RectTransform mapRect;

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
            case "event":
                if (message.payload.action == "collect_key")
                {
                    string keyId = message.payload.target;
                    FindObjectOfType<KeyTracker>()?.MarkKeyCollected(keyId);
                }
                break;

            case "position":
                UpdatePlayerIcon(message.payload);
                break;
            case "GeneratorCode":
                Debug.Log("New Code: " + message.payload.code);
                ChangeCode(message.payload.code);
                break;
            case "IndexCode":
                Debug.Log("si llega el mensaje " + message.payload.codeindex);
                break;
            default:
                Debug.LogWarning("Unknown message type: " + message.type);
                break;
        }
    }

    private void UpdatePlayerIcon(Payload payload)
    {
        Vector2 worldPos = new Vector2(payload.x, payload.z);
        Vector2 offset = new Vector2(-420f, 310f); // ajustar hasta que quede alineado
        
        float normX = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
        float normY = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.y);

        float mapWidth = mapRect.rect.width;
        float mapHeight = mapRect.rect.height;

        float posX = Mathf.Clamp(normX * mapWidth, 5f, mapWidth - 5f);
        float posY = -(1f - normY) * mapHeight;

        playerIcon.anchoredPosition = new Vector2(posX, posY) + offset;

        float rotationY = payload.rotationY;
        playerIcon.rotation = Quaternion.Euler(0, 0, -rotationY);

        //Debug.Log($"Icon updated: Pos({position2D}), Rot({rotationY})");
    }

    public void ChangeCode(string code){
        for (int i = 0; i < codeSlotsText.Length; i++){
            codeSlotsText[i].text = code[i].ToString();
        }
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

    public void SendOpenNearestDoorCommand()
    {
        var msg = new NetworkMessage
        {
            type = "command",
            payload = new Payload
            {
                action = "open_nearest_door"
            }
        };

        _socket.SendNetworkMessage(msg);
        Debug.Log("📨 Sent command: open_nearest_door");
    }

    public void SendCloseNearestDoorCommand()
    {
        var msg = new NetworkMessage
        {
            type = "command",
            payload = new Payload
            {
                action = "close_nearest_door"
            }
        };

        _socket.SendNetworkMessage(msg);
        Debug.Log("📨 Sent command: close_nearest_door");
    }

    public void SimularRecolectarLlave(string keyId)
    {
        var msgJson = JsonUtility.ToJson(new NetworkMessage
        {
            type = "event",
            payload = new Payload
            {
                action = "collect_key",
                target = keyId
            }
        });

        HandleServerMessage(msgJson);
    }
}
