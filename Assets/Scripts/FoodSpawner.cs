using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSpawner : MonoBehaviour
{
    public Transform objectPanel;
    public GameObject foodItemPrefab;
    public List<FoodData> foodDatabase;
    public Canvas canvas;
    public int totalToSpawn = 3;

    private List<FoodData> remainingFoods = new List<FoodData>();
    private List<GameObject> currentFoodItems = new List<GameObject>();
    private List<FoodData> usedFoods = new List<FoodData>();

    void Start()
    {
        ResetSpawner();
    }

    public void ResetSpawner()
    {
        remainingFoods = new List<FoodData>(foodDatabase);
        usedFoods.Clear();

        foreach (Transform child in objectPanel)
            Destroy(child.gameObject);

        currentFoodItems.Clear();

        for (int i = 0; i < totalToSpawn; i++)
        {
            SpawnOneNewFood();
        }

        Debug.Log("[RESET] Spawner direset. Total makanan: " + foodDatabase.Count);
    }

    public void SpawnOneNewFood()
    {
        if (remainingFoods.Count == 0)
        {
            Debug.Log("[SPAWN] Tidak ada makanan tersisa.");
            return;
        }

        int rand = Random.Range(0, remainingFoods.Count);
        FoodData selected = remainingFoods[rand];
        remainingFoods.RemoveAt(rand);
        usedFoods.Add(selected);

        GameObject foodGO = Instantiate(foodItemPrefab, objectPanel);
        foodGO.GetComponent<Image>().sprite = selected.foodSprite;

        var drag = foodGO.GetComponent<DraggableFood>();
        drag.foodData = selected;
        drag.canvas = canvas;
        drag.foodSpawner = this;

        currentFoodItems.Add(foodGO);

        Debug.Log($"[SPAWN] Menambahkan: {selected.foodName} | Sehat: {selected.isHealthy} | Tersisa: {remainingFoods.Count}");
    }

    public void ReplaceFood(GameObject oldFood, FoodData oldData)
    {
        if (oldFood == null)
        {
            Debug.LogWarning("[REPLACE] oldFood = NULL");
            return;
        }

        Debug.Log($"[REPLACE] Menghapus: {oldData.foodName}");

        currentFoodItems.Remove(oldFood);
        usedFoods.Remove(oldData);
        Destroy(oldFood);

        SpawnOneNewFood();
    }
}
