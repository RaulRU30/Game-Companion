using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelDashboard;
    public GameObject panelGame1;
    public GameObject panelGame2;

    public void OpenPanelGame1()
    {
        panelDashboard.SetActive(false);
        panelGame1.SetActive(true);
    }

    public void OpenPanelGame2()
    {
        panelDashboard.SetActive(false);
        panelGame2.SetActive(true);
    }

    public void ReturnToDashboard()
    {
        panelGame1.SetActive(false);
        panelGame2.SetActive(false);
        panelDashboard.SetActive(true);
    }
}
