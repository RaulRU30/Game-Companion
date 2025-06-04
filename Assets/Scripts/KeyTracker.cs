using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyTracker : MonoBehaviour
{
    public KeyTracker keyTracker;
    [System.Serializable]
    public class KeyUI
    {
        public string keyId;
        public RectTransform keyIcon;
    }

    public KeyUI[] keys;

    public void MarkKeyCollected(string keyId)
    {
        foreach (var key in keys)
        {
            if (key.keyId == keyId)
            {
                key.keyIcon.gameObject.SetActive(false);
                Debug.Log($"🔒 Llave oculta en minimapa: {keyId}");
                return;
            }
        }

        Debug.LogWarning($"⚠ Llave con ID '{keyId}' no encontrada en minimapa.");
    }

    public void MostrarTodasLasLlaves()
    {
        foreach (var key in keys)
        {
            if (key.keyIcon != null)
                key.keyIcon.gameObject.SetActive(true);
        }

        Debug.Log("🔓 Llaves visibles en el minimapa");
    }
}
