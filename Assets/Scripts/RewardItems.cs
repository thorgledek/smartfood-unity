using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardItemUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button claimButton;

    private int requiredPoints;
    private RewardUIController uiController;

    public void Setup(string title, string description, Sprite icon, int points, RewardUIController controller)
    {
        titleText.text = $"Collect {points} Points";
        descriptionText.text = description;
        iconImage.sprite = icon;

        requiredPoints = points;
        uiController = controller;

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(OnClaimClicked);
    }

    private void OnClaimClicked()
    {
        if (uiController.CanClaim(requiredPoints))
        {
            uiController.DeductPoints(requiredPoints);
            Debug.Log("Reward claimed!");
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }
}
