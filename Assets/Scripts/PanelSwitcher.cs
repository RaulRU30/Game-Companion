using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelDashboard;
    public GameObject panelGame1;
    public GameObject panelGame2;
<<<<<<< HEAD
    public GameObject minimap;
=======
    public KeyTracker keyTracker;
>>>>>>> b1244a1516988fff1e1d8eebee47d0f09eb62e59
    public GameManager gameManager;
    public GameObject game2Location;

    public void OpenPanelGame1()
    {
        minimap.SetActive(true);
        game2Location.SetActive(false);
        panelDashboard.SetActive(false);
        panelGame1.SetActive(true);

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
        minimap.SetActive(false);
        game2Location.SetActive(false);
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
