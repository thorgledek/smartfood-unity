using System.Collections;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using UnityEngine.SceneManagement;

public class RegisterHandler : MonoBehaviour
{
    
    public TMP_InputField inputUsername;
    public TMP_InputField inputPassword;
    public TMP_InputField inputAge;
    public TMP_InputField inputHeight;
    public TMP_InputField inputWeight;

    
    public TextMeshProUGUI statusMessageText; 
    public GameObject registerButton; 

    
    private const string REGISTER_URL = "http://localhost/LoginFolder/Register.php"; 

    void Start()
    {
        
        if (statusMessageText != null)
        {
            statusMessageText.text = "";
        }
    }

    
    public void OnRegisterButtonClick()
    {
        
        if (registerButton != null)
        {
            registerButton.GetComponent<Button>().interactable = false;
        }

        
        if (string.IsNullOrEmpty(inputUsername.text) || string.IsNullOrEmpty(inputPassword.text) ||
            string.IsNullOrEmpty(inputAge.text) || string.IsNullOrEmpty(inputHeight.text) ||
            string.IsNullOrEmpty(inputWeight.text))
        {
            DisplayStatusMessage("Lengkapi Kolom Diatas!", Color.red);
            ReenableRegisterButton();
            return;
        }

        // Pastikan Age, Height, Weight adalah angka
        if (!int.TryParse(inputAge.text, out int ageValue) ||
            !float.TryParse(inputHeight.text, out float heightValue) ||
            !float.TryParse(inputWeight.text, out float weightValue))
        {
            DisplayStatusMessage("Age, Height, Weight harus berupa angka!", Color.red);
            ReenableRegisterButton();
            return;
        }

        StartCoroutine(SendRegistrationRequest(inputUsername.text, inputPassword.text, ageValue, heightValue, weightValue));
    }

    IEnumerator SendRegistrationRequest(string username, string password, int age, float height, float weight)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("age", age.ToString()); // Kirim sebagai string
        form.AddField("height", height.ToString()); // Kirim sebagai string
        form.AddField("weight", weight.ToString()); // Kirim sebagai string

        using (UnityEngine.Networking.UnityWebRequest webRequest = UnityEngine.Networking.UnityWebRequest.Post(REGISTER_URL, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                DisplayStatusMessage("Koneksi gagal: " + webRequest.error, Color.red);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                Debug.Log("Server Response: " + responseText);

                if (responseText.Contains("Registrasi Berhasil!"))
                {
                    DisplayStatusMessage("Registrasi Berhasil!", Color.green);
                    inputUsername.text = "";
                    inputPassword.text = "";
                    inputAge.text = "";
                    inputHeight.text = "";
                    inputWeight.text = "";

                    SceneManager.LoadScene("Menu Utama");
                }
                else if (responseText.Contains("Username sudah terdaftar."))
                {
                    DisplayStatusMessage("Username sudah terdaftar. Gunakan username lain.", Color.red);
                }
                else
                {
                    DisplayStatusMessage("Registrasi gagal: " + responseText, Color.red);
                }


                if (responseText.Contains("Registrasi Berhasil!")) // Jangan taruh dua kali!
                {
                    DisplayStatusMessage("Registrasi Berhasil!", Color.green);

                    PlayerPrefs.SetString("LoggedInUsername", username);
                    PlayerPrefs.Save();

                    SceneManager.LoadScene("Menu Utama");
                }

            }
        }
        ReenableRegisterButton();
    }

    private void DisplayStatusMessage(string message, Color color)
    {
        if (statusMessageText != null)
        {
            statusMessageText.text = message;
            statusMessageText.color = color;
        }
    }

    private void ReenableRegisterButton()
    {
        if (registerButton != null)
        {
            registerButton.GetComponent<Button>().interactable = true;
        }
    }


}