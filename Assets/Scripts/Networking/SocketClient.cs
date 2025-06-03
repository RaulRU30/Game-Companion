using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Networking
{
    public class SocketClient : MonoBehaviour
    {
        private TcpClient _client;
        private NetworkStream _stream;
    
        public Action<string> OnMessageReceived;
        public Action OnConnected;

        public void ConnectToServer(string ip) {
            try {
                _client = new TcpClient();
                _client.Connect(ip, 1337);
                _stream = _client.GetStream();
                
                OnConnected?.Invoke();
                
                Debug.Log("Connected to server at: " + ip);
                StartCoroutine(ListenToServer());
            
            } catch (SocketException ex) {
                Debug.LogError("Failed to connect: " + ex.Message);
            }
        }
        
        public void SendNetworkMessage(NetworkMessage message) {
            if (_client is not { Connected: true }) return;

            string json = JsonUtility.ToJson(message);
            byte[] data = Encoding.ASCII.GetBytes(json);
            _stream.Write(data, 0, data.Length);
            Debug.Log("Sent: " + json);
        }

        
        public void SendMessageToServer(string message) {
            if (_client is not { Connected: true }) {
                Debug.LogWarning("⚠️ No hay conexión activa con el servidor.");
                return;
            }

            if (_stream is { CanWrite: true }) {
                byte[] data = Encoding.ASCII.GetBytes(message);
                _stream.Write(data, 0, data.Length);
                Debug.Log("📤 Mensaje enviado: " + message);
            } else {
                Debug.LogWarning("⚠️ Stream no disponible para escritura.");
            }
        }
    
        private IEnumerator ListenToServer() {
            StreamReader reader = new StreamReader(_stream, Encoding.ASCII);


            while (_client is { Connected: true }) {
                if (_stream.DataAvailable) {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line)) {
//                        Debug.Log("📥 Received: " + line);
                        OnMessageReceived?.Invoke(line);
                    }
                }

                yield return null;
            }
        }



        public void SendTestCommand() {
            SendMessageToServer("open_door");
        }

        private void OnApplicationQuit() {
            _stream?.Close();
            _client?.Close();
        }

    }
}
