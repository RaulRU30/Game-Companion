using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTracker : MonoBehaviour
{
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
}
