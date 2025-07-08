using UnityEngine;

[CreateAssetMenu(fileName = "FoodItem", menuName = "Makanan/FoodItem", order = 1)]
public class FoodItem : ScriptableObject
{
    public string foodName;
    public Sprite icon;
    public int calories;
}
