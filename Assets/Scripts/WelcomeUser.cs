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
            string username = PlayerPrefs.GetString("username", "User"); // ✅ pakai key yang benar
            welcomeUsernameDisplay.text = "Welcome, " + username;
        }
    }

    private string GetGreetingBasedOnTime()
    {
        int hour = System.DateTime.Now.Hour;

        if (hour >= 5 && hour < 12) return "Good Morning";
        else if (hour >= 12 && hour < 15) return "Good Afternoon";
        else if (hour >= 15 && hour < 18) return "Good Evening";
        else if (hour >= 18 && hour < 22) return "Good Night";
        else return "It's Time To Sleep";
    }

    private void ShowWelcomePopupWithUsername()
    {
        string username = PlayerPrefs.GetString("username", "Pengguna"); // ✅ pakai key yang benar
        string greeting = GetGreetingBasedOnTime();

        if (welcomePopupPanel != null && welcomeMessageText != null)
        {
            welcomeMessageText.text = $"{greeting}, {username}";
            welcomePopupPanel.SetActive(true);
        }
    }
}
