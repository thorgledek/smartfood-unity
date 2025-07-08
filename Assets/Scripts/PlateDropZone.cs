using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlateDropZone : MonoBehaviour, IDropHandler
{
    [Header("Item Slot Maksimum 3")]
    [SerializeField] private Transform[] itemSlots;

    [Header("Kalori Text (TextMeshProUGUI)")]
    [SerializeField] private TextMeshProUGUI kaloriText;

    private int totalCalories = 0;
    private List<FoodData> placedFoods = new List<FoodData>();

    public void OnDrop(PointerEventData eventData)
    {
        if (placedFoods.Count >= 3) return;

        var drag = eventData.pointerDrag;
        if (drag == null) return;

        if (drag.TryGetComponent<FoodDragHandler>(out var food))
        {
            if (food.foodData == null)
            {
                Debug.LogWarning("FoodData pada prefab tidak terisi!");
                return;
            }

            if (!placedFoods.Contains(food.foodData))
            {
                placedFoods.Add(food.foodData);
                totalCalories += food.foodData.calories;

                if (kaloriText != null)
                    kaloriText.text = $"Kalori: {totalCalories}";
                else
                    Debug.LogWarning("Kalori Text belum ditemukan!");

                // Tampilkan icon makanan di slot kosong
                for (int i = 0; i < itemSlots.Length; i++)
                {
                    if (itemSlots[i].childCount == 0)
                    {
                        GameObject newIcon = new GameObject(food.foodData.foodName, typeof(Image));
                        newIcon.transform.SetParent(itemSlots[i], false);

                        Image img = newIcon.GetComponent<Image>();
                        img.sprite = food.foodData.foodSprite;
                        img.preserveAspect = true;

                        break;
                    }
                }
            }
        }
    }

    public void ResetPlate()
    {
        foreach (var slot in itemSlots)
        {
            foreach (Transform child in slot)
                Destroy(child.gameObject);
        }

        placedFoods.Clear();
        totalCalories = 0;

        if (kaloriText != null)
            kaloriText.text = "Kalori: 0";
        else
            Debug.LogWarning("Kalori Text belum ditemukan!");
    }
}
