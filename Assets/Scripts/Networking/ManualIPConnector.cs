using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class ManualIPConnector : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_InputField ipInputField;
        [SerializeField] private Button connectButton;
        [SerializeField] private TMP_Text statusText;
        
        [Header("Panels")]
        [SerializeField] private GameObject panelConnect;
        [SerializeField] private GameObject panelDashboard;
        
        [Header("Networking")]
        [SerializeField] private SocketClient socketClient;
        
        private const string SavedIPKey = "SavedServerIP";


        void Start()
        {
            if (ipInputField == null || connectButton == null || socketClient == null) {
                Debug.LogError("ManualIPConnector: Missing references.");
                return;
            }

            if (PlayerPrefs.HasKey(SavedIPKey)) {
                ipInputField.text = PlayerPrefs.GetString(SavedIPKey);
            }
            
            panelConnect.SetActive(true);
            panelDashboard.SetActive(false);

            connectButton.onClick.AddListener(() => {
                string ip = ipInputField.text.Trim();

                if (string.IsNullOrEmpty(ip)) {
                    statusText.text = "Please enter a valid IP.";
                    return;
                }

                PlayerPrefs.SetString(SavedIPKey, ip);
                PlayerPrefs.Save();

                statusText.text = "Connecting to " + ip + "...";
                socketClient.ConnectToServer(ip);
            });
            
            socketClient.OnConnected += OnSuccessfulConnection;

        }
        
        private void OnSuccessfulConnection()
        {
            statusText.text = "✅ Connected!";
            panelConnect.SetActive(false);
            panelDashboard.SetActive(true);
        }


    }
}
