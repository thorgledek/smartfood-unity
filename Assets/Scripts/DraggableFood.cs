using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public FoodData foodData;
    public Canvas canvas;
    public FoodSpawner foodSpawner;
    public DropTarget dropTarget;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    public bool hasBeenDropped = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Cek dan ambil CanvasGroup, pastikan tidak null
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("[DraggableFood] CanvasGroup tidak ditemukan di " + gameObject.name);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        // Bawa ke atas canvas agar tidak tertutup UI lain
        transform.SetParent(canvas.transform);
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;

        hasBeenDropped = false;

        Debug.Log($"[DRAG] Mulai drag: {foodData.foodName}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (hasBeenDropped && dropTarget != null)
        {
            // Hapus & ganti di sini karena pasti sudah didrop
            foodSpawner.ReplaceFood(gameObject, foodData);
            dropTarget = null;
        }
        else
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
            Debug.Log($"[DRAG] {foodData.foodName} tidak dijatuhkan, kembali ke posisi awal.");
        }

        hasBeenDropped = false;
    }

}
