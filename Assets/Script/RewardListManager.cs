using System.Collections.Generic;
using UnityEngine;

public class RewardListManager : MonoBehaviour
{
    public GameObject rewardItemPrefab;
    public Transform contentParent;
    public RewardUIController uiController;

    private List<RewardData> rewards = new List<RewardData>(); // ✅ Tambahkan ini!

    public void LoadRewardsFromServer(List<RewardData> serverRewards)
    {
        Debug.Log("Memuat " + serverRewards.Count + " reward dari server");

        rewards = serverRewards; // ✅ Simpan datanya ke field `rewards`

        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var reward in rewards)
        {
            Debug.Log("Reward: " + reward.title + " stok: " + reward.stock);

            if (reward.stock <= 0)
                continue;

            GameObject item = Instantiate(rewardItemPrefab, contentParent);
            RewardItemUI itemUI = item.GetComponent<RewardItemUI>();

            Sprite iconSprite = Resources.Load<Sprite>(reward.icon_url.Replace(".png", ""));
            itemUI.Setup(reward.title, reward.deskripsi, iconSprite, reward.require_points, uiController);
        }
    }

    public void GenerateRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.stock <= 0)
                continue;

            Debug.Log("Spawning: " + reward.title);

            GameObject item = Instantiate(rewardItemPrefab, contentParent);
            RewardItemUI itemUI = item.GetComponent<RewardItemUI>();

            Sprite iconSprite = Resources.Load<Sprite>(reward.icon_url.Replace(".png", ""));
            itemUI.Setup(reward.title, reward.deskripsi, iconSprite, reward.require_points, uiController);
        }
    }
}
