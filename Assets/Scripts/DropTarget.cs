using UnityEngine;
using UnityEngine.EventSystems;

public class DropTarget : MonoBehaviour, IDropHandler
{
    public bool isHealthyZone;
    public GameManager gameManager;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableFood dropped = eventData.pointerDrag?.GetComponent<DraggableFood>();
        if (dropped != null)
        {
            bool correct = dropped.foodData.isHealthy == isHealthyZone;
            gameManager.CheckAnswer(correct);

            dropped.hasBeenDropped = true;
            dropped.dropTarget = this;
            Debug.Log($"[DROP] {dropped.foodData.foodName} berhasil dijatuhkan ke zone: {isHealthyZone}");
        }
    }


}
