using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class PaymentHandler : MonoBehaviour
{
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI totalCostText;
    public TextMeshProUGUI pointText;
    public TextMeshProUGUI descText;
    public Image iconImage;
    public TextMeshProUGUI angkaTotalText;
    public TextMeshProUGUI angkaPointText;
    public TMP_InputField alamatInputField;

    public Button paymentButton;

    private int currentPoints;
    private int itemCost;
    private string username;

    void Start()
    {
        // Ambil data dari SelectedReward static class
        if (string.IsNullOrEmpty(SelectedReward.itemName))
        {
            Debug.LogError("SelectedReward belum diisi!");
            return;
        }

        if (itemText != null) itemText.text = SelectedReward.itemName;
        if (descText != null) descText.text = SelectedReward.itemDesc;
        if (iconImage != null) iconImage.sprite = SelectedReward.itemIcon;

        itemCost = SelectedReward.itemCost;
        if (totalCostText != null) totalCostText.text = itemCost.ToString();
        if (angkaTotalText != null) angkaTotalText.text = itemCost.ToString();

        username = PlayerPrefs.GetString("username", "");
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username belum disimpan di PlayerPrefs!");
            return;
        }

        if (paymentButton != null)
            paymentButton.onClick.AddListener(HandlePayment);

        StartCoroutine(LoadPointsFromServer());
    }

    IEnumerator LoadPointsFromServer()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/LoginFolder/GetPoints.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseText = www.downloadHandler.text.Trim();
            Debug.Log("Response poin dari server: [" + responseText + "]");

            if (int.TryParse(responseText, out currentPoints))
            {
                if (pointText != null)
                    pointText.text = currentPoints.ToString();

                if (angkaPointText != null)
                    angkaPointText.text = currentPoints.ToString();
            }
            else
            {
                Debug.LogError("Gagal parsing poin: [" + responseText + "]");
            }
        }
        else
        {
            Debug.LogError("Error GetPoints.php: " + www.error);
        }
    }

    void HandlePayment()
    {
        string alamat = alamatInputField.text.Trim();

        if (string.IsNullOrEmpty(alamat))
        {
            Debug.Log("Alamat harus diisi!");
            // Bisa juga tampilkan popup atau UI warning di sini
            return;
        }

        if (currentPoints < itemCost)
        {
            Debug.Log("Point tidak cukup!");
            // Bisa juga tampilkan popup atau UI warning di sini
            return;
        }

        // Jika semua valid, lakukan pembayaran
        StartCoroutine(ProcessPayment(alamat));
    }

    IEnumerator UpdatePointsOnServer(int delta)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("points", delta);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/LoginFolder/UpdatePoints.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sukses update poin: " + www.downloadHandler.text);
            StartCoroutine(LoadPointsFromServer());
        }
        else
        {
            Debug.LogError("UpdatePoints gagal: " + www.error);
        }
    }
    IEnumerator ProcessPayment(string alamat)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("points", -itemCost);   // pengurangan poin
        form.AddField("alamat", alamat);      // kirim alamat

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/LoginFolder/UpdatePointsAndAddress.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Pembayaran sukses: " + www.downloadHandler.text);
            StartCoroutine(LoadPointsFromServer()); // update poin terbaru
                                                    // Bisa lanjut ke scene lain atau tampilkan sukses
        }
        else
        {
            Debug.LogError("Pembayaran gagal: " + www.error);
        }
    }

}
