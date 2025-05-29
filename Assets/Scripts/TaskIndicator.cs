using UnityEngine;
using UnityEngine.UI;

public class TaskIndicator : MonoBehaviour
{
    public Image indicatorImage;
    public Color colorPendiente = Color.red;
    public Color colorCompletado = Color.green;

    private bool completado = false;

    void Start()
    {
        // Inicia en rojo (pendiente)
        indicatorImage.color = colorPendiente;
    }

    // Cambia entre pendiente <-> completado (por clic manual, si lo usas)
    public void CambiarEstado()
    {
        completado = !completado;
        indicatorImage.color = completado ? colorCompletado : colorPendiente;
    }

    // Forzar a completado desde otro script (como GameTimer)
    public void MarcarCompletado()
    {
        completado = true;
        indicatorImage.color = colorCompletado;
    }

    // (opcional) Resetear estado
    public void Resetear()
    {
        completado = false;
        indicatorImage.color = colorPendiente;
    }
}
