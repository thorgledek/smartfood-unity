using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RewardFetcher : MonoBehaviour
{
    public string url = "http://localhost/LoginFolder/GetRewards.php";
    public RewardListManager rewardListManager;

    void Start()
    {
        StartCoroutine(GetRewardsFromServer());
    }

    IEnumerator GetRewardsFromServer()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch rewards: " + www.error);
        }
        else
        {
            // Ganti bagian ini
            RewardData[] rewardArray = JsonHelper.FromJson<RewardData>(www.downloadHandler.text);
            Debug.Log("Jumlah reward dari server: " + rewardArray.Length);

            rewardListManager.LoadRewardsFromServer(new List<RewardData>(rewardArray));
        }
    }
}
