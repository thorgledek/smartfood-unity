using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public string targetSceneName;

    public void GoToScene()
    {
        // Simpan scene saat ini ke PlayerPrefs
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(targetSceneName);
    }

    public void BackToPreviousScene()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", "Menu Utama"); // default jika tidak ditemukan
        SceneManager.LoadScene(lastScene);
    }
}
