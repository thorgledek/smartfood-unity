using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizPopupController : MonoBehaviour
{
    public GameObject popupPanel;      // Panel "Are you ready?"
    public GameObject quizPanel;       // Panel pertanyaan
    public string previousSceneName;   // Nama scene sebelum ini

    void Start()
    {
        ShowPopup(); // 🔥 Tambahkan baris ini!
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        quizPanel.SetActive(false);   // Pastikan quiz belum tampil
    }

    public void StartQuiz()
    {
        popupPanel.SetActive(false);  // Sembunyikan popup
        quizPanel.SetActive(true);    // Tampilkan quiz
    }

    public void CancelQuiz()
    {
        SceneManager.LoadScene(previousSceneName); // Kembali ke scene sebelumnya
    }
}
