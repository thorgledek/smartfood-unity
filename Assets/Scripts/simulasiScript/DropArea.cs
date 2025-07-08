using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropArea : MonoBehaviour, IDropHandler
{
    public Transform foodContainer;
    public TextMeshProUGUI totalCaloriesText;
    public foodSpawner spawner; // ✅ Tambahkan ini

    private int totalCalories = 0;
    private int maxItems = 3;

    public void OnDrop(PointerEventData eventData)
    {
        // Kosong, logika drag ada di DraggableItem
    }

    public void OnItemDropped(DraggableItem item)
    {
        totalCalories += item.calories;
        totalCaloriesText.text = totalCalories + " kcal";
    }

    public void ResetArea()
    {
        // 🔁 1. Reset makanan yang sudah di-drop
        foreach (Transform child in foodContainer)
        {
            Destroy(child.gameObject);
        }

        // 🔁 2. Reset kalori
        totalCalories = 0;
        totalCaloriesText.text = "0 kcal";

        // 🔁 3. Ganti isi foodPanel dengan makanan baru
        if (spawner != null)
            spawner.SpawnFood();
    }

    public bool CanAcceptMore()
    {
        return foodContainer.childCount < maxItems;
    }
}
