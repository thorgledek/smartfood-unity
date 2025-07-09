using System.Collections.Generic;

[System.Serializable]
public class RewardData
{
    public string title;
    public string deskripsi;
    public string icon_url;
    public int require_points;
    public int stock;
}

[System.Serializable]
public class RewardList
{
    public RewardData[] rewards;
}

[System.Serializable]
public class RewardListWrapper
{
    public List<RewardData> rewards;
}
