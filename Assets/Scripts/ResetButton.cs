using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public PlateDropZone plateZone;

    public void OnResetClick()
    {
        plateZone.ResetPlate();
    }
}
