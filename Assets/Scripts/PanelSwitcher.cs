using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelDashboard;
    public GameObject panelGame1;
    public GameObject panelGame2;
    public GameObject minimap;
    public GameManager gameManager;
    public GameObject game2Location;

    public void OpenPanelGame1()
    {
        minimap.SetActive(true);
        game2Location.SetActive(false);
        panelDashboard.SetActive(false);
        panelGame1.SetActive(true);
    }

    public void OpenPanelGame2()
    {
        minimap.SetActive(false);
        game2Location.SetActive(false);
        panelDashboard.SetActive(false);
        panelGame2.SetActive(true);
        gameManager.SendActive(1);
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
