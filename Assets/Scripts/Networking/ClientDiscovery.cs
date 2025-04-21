using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Networking
{
    public class ClientDiscovery : MonoBehaviour
    {
        public System.Action<string> OnServerFound;
    
        public void StartDiscovery() {
            StartCoroutine(DiscoverServer());
        }
    
        IEnumerator DiscoverServer() {
            UdpClient udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            udpClient.Client.ReceiveTimeout = 2000;

            byte[] requestData = Encoding.UTF8.GetBytes("hello_server");
            IPEndPoint broadcastEP = new IPEndPoint(IPAddress.Broadcast, 8888);
            udpClient.Send(requestData, requestData.Length, broadcastEP);

            Debug.Log("Broadcast sent...");

            const float timeout = 2f;
            float timer = 0f;

            while (timer < timeout) {
                if (udpClient.Available > 0) {
                    IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] response = udpClient.Receive(ref serverEP);

                    string msg = Encoding.UTF8.GetString(response);
                    if (msg == "server_here") {
                        Debug.Log($"Server found at: {serverEP.Address}");
                    
                        OnServerFound?.Invoke(serverEP.Address.ToString());
                        break;
                    }
                }

                timer += Time.deltaTime;
                yield return null;
            }

            udpClient.Close();
        }

    }
}
