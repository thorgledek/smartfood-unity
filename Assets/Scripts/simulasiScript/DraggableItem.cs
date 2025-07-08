using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public int calories;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root, true); // pindah ke atas canvas
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter;
        DropArea dropArea = target != null ? target.GetComponentInParent<DropArea>() : null;

        if (dropArea != null && dropArea.CanAcceptMore())
        {
            transform.SetParent(dropArea.foodContainer, false); // tetap di UI hierarchy
            rectTransform.localScale = Vector3.one;
            canvasGroup.blocksRaycasts = true;

            dropArea.OnItemDropped(this); // panggil untuk update kalori
        }
        else
        {
            // Kembali ke posisi semula
            transform.SetParent(originalParent, false);
            canvasGroup.blocksRaycasts = true;
        }
    }
}
