using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelDashboard;
    public GameObject panelGame1;
    public GameObject panelGame2;
    public GameObject minimap;
    public KeyTracker keyTracker;
    public GameManager gameManager;
    public GameObject game2Location;
    public GameObject keyManager;

    public void OpenPanelGame1()
    {
        game2Location.SetActive(false);
        minimap.SetActive(true);
        panelDashboard.SetActive(false);
        panelGame1.SetActive(true);
        keyManager.SetActive(true);

        if (keyTracker != null)
            keyTracker.MostrarTodasLasLlaves();

        if (gameManager != null)
        {
            Debug.Log("📲 Enviando mensaje de inicio de minijuego desde PanelSwitcher");
            gameManager.SendStartKeyGameCommand();
        }
        else
        {
            Debug.LogError("❌ GameManager no está asignado en PanelSwitcher");
        }
    }

    public void OpenPanelGame2()
    {
        game2Location.SetActive(false);
        minimap.SetActive(false);
        panelDashboard.SetActive(false);
        panelGame2.SetActive(true);
    }

    public void ReturnToDashboard()
    {
        game2Location.SetActive(true);
        minimap.SetActive(true);
        panelGame1.SetActive(false);
        panelGame2.SetActive(false);
        panelDashboard.SetActive(true);
    }
}
