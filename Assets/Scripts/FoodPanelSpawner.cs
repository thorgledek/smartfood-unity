using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FoodPanelSpawner : MonoBehaviour
{
    public GameObject foodItemPrefab;
    public Transform parentPanel;
    public List<FoodData> foodList; // Isi di Inspector

    void Start()
    {
        foreach (var food in foodList)
        {
            GameObject item = Instantiate(foodItemPrefab, parentPanel);
            Image img = item.GetComponent<Image>();
            img.sprite = food.foodSprite;

            FoodDragHandler drag = item.GetComponent<FoodDragHandler>();
            drag.foodData = food; // Ini satu-satunya yang diperlukan
        }
    }
}
