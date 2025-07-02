using UnityEngine;
using TMPro;

public class WelcomeUser : MonoBehaviour
{
    [Header("Popup Selamat Datang")]
    public GameObject welcomePopupPanel; // Panel popup selamat datang
    public TextMeshProUGUI welcomeMessageText; // Teks utama dalam popup

    [Header("Teks Username di UI lain (opsional)")]
    public TextMeshProUGUI welcomeUsernameDisplay; // Misal di pojok atas UI statis

    void Start()
    {
        // Sembunyikan popup lebih dulu
        if (welcomePopupPanel != null)
            welcomePopupPanel.SetActive(false);

        // Tampilkan username di UI statis jika ada
        DisplayUsernameOnStaticUI();

        // Tampilkan popup selamat datang
        ShowWelcomePopupWithUsername();
    }

    private void DisplayUsernameOnStaticUI()
    {
        if (welcomeUsernameDisplay != null)
        {
            string username = PlayerPrefs.GetString("LoggedInUsername", "User");
            welcomeUsernameDisplay.text = "Welcome, " + username;
        }
    }

    private string GetGreetingBasedOnTime()
    {
        int hour = System.DateTime.Now.Hour;

        if (hour >= 5 && hour < 12) return "Selamat Pagi";
        else if (hour >= 12 && hour < 15) return "Selamat Siang";
        else if (hour >= 15 && hour < 18) return "Selamat Sore";
        else return "Selamat Malam";
    }

    private void ShowWelcomePopupWithUsername()
    {
        string username = PlayerPrefs.GetString("LoggedInUsername", "Pengguna");
        string greeting = GetGreetingBasedOnTime();

        if (welcomePopupPanel != null && welcomeMessageText != null)
        {
            welcomeMessageText.text = $"{greeting}, {username}";
            welcomePopupPanel.SetActive(true);
        }
    }
}
