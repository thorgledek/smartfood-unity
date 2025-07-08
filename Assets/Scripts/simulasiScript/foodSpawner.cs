using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class foodSpawner : MonoBehaviour
{
    public Sprite[] foodSprites;
    public int[] foodCalories;
    public GameObject foodPrefab;
    public Transform foodPanel;

    private List<int> lastSpawnedIndexes = new List<int>();

    void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        // Hapus makanan lama
        foreach (Transform child in foodPanel)
        {
            Destroy(child.gameObject);
        }

        lastSpawnedIndexes.Clear();

        int maxItemsToSpawn = Mathf.Min(3, foodSprites.Length);
        while (lastSpawnedIndexes.Count < maxItemsToSpawn)
        {
            int index = Random.Range(0, foodSprites.Length);
            if (!lastSpawnedIndexes.Contains(index))
            {
                lastSpawnedIndexes.Add(index);

                GameObject newFood = Instantiate(foodPrefab, foodPanel);
                newFood.GetComponent<Image>().sprite = foodSprites[index];

                var di = newFood.GetComponent<DraggableItem>();
                di.calories = foodCalories[index];
            }
        }
    }
}
