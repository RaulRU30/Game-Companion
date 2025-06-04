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
     public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
     public Image[] codeSlotsImage;
    public Image game2Location;
    public Image successIndicator;
    public TaskIndicator[] taskIndicators;
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
            switch (message.payload.action)
            {
                case "collect_key":
                    Debug.Log($"📩 Recibido: collect_key {message.payload.target}");
                    OcultarIconoLlave(message.payload.target);
                    break;
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
                Debug.Log("indice " + message.payload.codeindex + " estado " + message.payload.state);
                if(message.payload.state == 1)//correcta    
                    codeSlotsImage[message.payload.codeindex].color = correctColor;
                if (message.payload.state == 2)//incorrecta
                    OnWrongInput();
                if (message.payload.state == 3)//tarea completada
                    OnCodeCompleted();
                break;
            case "TaskComplate":
                ChangeComplateTask(message.payload.state);
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
    private void OnCodeCompleted()
    {
        if (successIndicator != null)
            successIndicator.color = Color.green;
        ChangeComplateTask(1);
        Destroy(game2Location);
    }
    private void Reset()
    {
        for (int i = 0; i < codeSlotsImage.Length; i++)
        {
            codeSlotsImage[i].color = defaultColor;
        }
    }
    private void OnWrongInput()
    {
        for (int i = 0; i < codeSlotsImage.Length; i++)
        {
            codeSlotsImage[i].color = wrongColor;
        }

        Invoke(nameof(Reset), 1.0f);
    }
    private void ChangeComplateTask(int Taskindex)
    {
        taskIndicators[Taskindex].MarcarCompletado();
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

    public void SendStartCode()
    {
        var msg = new NetworkMessage
        {
            type = "StartCode",
            payload = new Payload
            {
                state=1,
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

    public void SendStartKeyGameCommand()
    {
        var msg = new NetworkMessage
        {
            type = "command",
            payload = new Payload
            {
                action = "start_key_game"
            }
        };

        _socket.SendNetworkMessage(msg);
        Debug.Log("📨 Enviado: start_key_game: " + msg.payload.action);
    }

    private void OcultarIconoLlave(string keyId)
    {
        Transform keysParent = GameObject.Find("Keys")?.transform;
        if (keysParent == null) return;

        Transform icono = keysParent.Find(keyId);
        if (icono != null)
            icono.gameObject.SetActive(false);
    }
}
