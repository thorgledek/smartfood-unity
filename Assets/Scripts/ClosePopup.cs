using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    public GameObject popupPanel; // Drag dan assign addpointsPopupPanel ke sini

    public void Close()
    {
        popupPanel.SetActive(false);
    }
}
