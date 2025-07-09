using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class BuyButtonHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image iconImage;

    public void OnBuyButtonClicked()
    {
        // Assign ke SelectedReward (static class)
        SelectedReward.itemName = titleText.text;
        SelectedReward.itemIcon = iconImage.sprite;

        // Ambil angka dari string costText, misal "100 Points" => "100"
        string digitsOnly = System.Text.RegularExpressions.Regex.Match(costText.text, @"\d+").Value;

        if (int.TryParse(digitsOnly, out int cost))
        {
            SelectedReward.itemCost = cost;
        }
        else
        {
            Debug.LogWarning("Gagal parsing cost dari costText: " + costText.text);
            SelectedReward.itemCost = 0;
        }

        // Load scene payment
        SceneManager.LoadScene("payment");
    }
}
