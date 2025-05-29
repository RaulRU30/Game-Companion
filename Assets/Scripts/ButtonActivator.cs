using UnityEngine;
using UnityEngine.UI;

public class ButtonActivator : MonoBehaviour
{
    public RectTransform playerIcon;

    public Button buttonGame1;
    public Button buttonGame2;
    // posiciones de las key
    public Vector2 point1 = new Vector2(-140, 153);
    public Vector2 point2 = new Vector2(-182, 60);
    public Vector2 point3 = new Vector2(-240, 20);
    //juego de los simbolos
    public Vector2 point4 = new Vector2(-100, -50); 

    public float activationRadius = 10f;

    void Update()
    {
        Vector2 playerPos = playerIcon.anchoredPosition;

        // Calcular distancias
        float distanceTo1 = Vector2.Distance(playerPos, point1);
        float distanceTo2 = Vector2.Distance(playerPos, point2);
        float distanceTo3 = Vector2.Distance(playerPos, point3);
        float distanceTo4 = Vector2.Distance(playerPos, point4);

        // Activar botón Game1 si está cerca de cualquiera de las 3 zonas
        bool nearAnyGame1Zone = distanceTo1 <= activationRadius || distanceTo2 <= activationRadius || distanceTo3 <= activationRadius;
        buttonGame1.gameObject.SetActive(nearAnyGame1Zone);

        // Activar botón Game2 solo si está cerca del punto 4
        buttonGame2.gameObject.SetActive(distanceTo4 <= activationRadius);
    }
}
