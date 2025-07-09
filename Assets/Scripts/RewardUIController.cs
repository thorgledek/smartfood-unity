using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RewardUIController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text balanceText;
    public TMP_Text remainingText;
    public Slider progressBar;
    public TMP_Text percentText;
    public TMP_Text messageText;

    [Header("Kuis Popup")]
    public GameObject quizPopupPanel; // drag panel popup kuis ke sini di Inspector

    [Header("Point Data")]
    public int currentPoints = 0;
    public int maxPoints = 3000;

    void Start()
    {
        StartCoroutine(FetchPointsFromDatabase());
    }

    IEnumerator FetchPointsFromDatabase()
    {
        string username = PlayerPrefs.GetString("LoggedInUsername", "Unknown");

        WWWForm form = new WWWForm();
        form.AddField("username", username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/LoginFolder/GetPoints.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                if (int.TryParse(response, out currentPoints))
                {
                    UpdateUI();
                }
                else
                {
                    Debug.LogWarning("Format poin tidak valid: " + response);
                }
            }
            else
            {
                Debug.LogError("Gagal mengambil poin: " + www.error);
            }
        }
    }

    public void UpdateUI()
    {
        if (balanceText == null || progressBar == null || remainingText == null || messageText == null)
        {
            Debug.LogError("UI references belum diisi di Inspector!");
            return;
        }

        balanceText.text = $"{currentPoints}";

        float percent = (float)currentPoints / maxPoints;
        progressBar.value = percent;

        if (currentPoints >= maxPoints)
        {
            messageText.text = "You Already Reached Your Target!";
            messageText.gameObject.SetActive(true);
            remainingText.gameObject.SetActive(false);
        }
        else
        {
            remainingText.text = $"{maxPoints - currentPoints} more points to collect your reward";
            messageText.gameObject.SetActive(false);
            remainingText.gameObject.SetActive(true);
        }
    }


    public void AddPoints(int points)
    {
        currentPoints += points;
        UpdateUI();
    }

    public bool CanClaim(int points)
    {
        return currentPoints >= points;
    }

    public void DeductPoints(int points)
    {
        currentPoints -= points;
        UpdateUI();
    }

    // Fungsi untuk menampilkan panel kuis (dipanggil saat tombol PointBalance diklik)
    public void ShowQuizPopup()
    {
        if (quizPopupPanel != null)
        {
            quizPopupPanel.SetActive(true);
        }
    }

    // Fungsi untuk menyembunyikan panel kuis
    public void HideQuizPopup()
    {
        if (quizPopupPanel != null)
        {
            quizPopupPanel.SetActive(false);
        }
    }
}
