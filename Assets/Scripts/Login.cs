using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public TMP_InputField inputUsername;
    public TMP_InputField inputPassword;

    IEnumerator ieLogin()
    {
        WWWForm dataForm = new WWWForm();
        dataForm.AddField("userName", inputUsername.text);
        dataForm.AddField("passWord", inputPassword.text);

        string uri = "http://localhost/LoginFolder/Login.php";

        UnityWebRequest webRequest = UnityWebRequest.Post(uri, dataForm);

        yield return webRequest.SendWebRequest();

        Debug.Log(webRequest.downloadHandler.text);

        string responseText = webRequest.downloadHandler.text;

        if (responseText.Trim() == "Berhasil")
        {
            Debug.Log("Login Berhasil!");
            PlayerPrefs.SetString("username", inputUsername.text);  // Konsisten pakai key "username"
            PlayerPrefs.Save();
            SceneManager.LoadScene("Menu Utama");
        }
        else
        {
            Debug.LogWarning("Login gagal: " + responseText);
        }
    }

    public void login()
    {
#if UNITY_EDITOR
        var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
#endif
        if (string.IsNullOrEmpty(inputUsername.text) || string.IsNullOrEmpty(inputPassword.text))
        {
            Debug.LogWarning("Username or Password is empty!");
            return;
        }
        StartCoroutine(ieLogin());
    }
}
