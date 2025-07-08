using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class QuizUIController : MonoBehaviour
{
    [Header("Global Timer")]
    public TMP_Text timerText;
    public float totalQuizTime = 60f;
    public CircularTimer circularTimer;

    private float remainingTime;
    private bool quizOngoing = true;

    [Header("UI Elements")]
    public TMP_Text questionText;
    public TMP_Text[] optionTexts;
    public TMP_Text questionNumberText;
    public Image[] resultIcons;
    public GameObject resultPanel;
    public TMP_Text resultPointText;

    private int score = 0;
    private int pointPerCorrect = 10;

    private List<Question> questions = new List<Question>();
    private int currentQuestionIndex = 0;

    void Start()
    {
        LoadQuestions();
        DisplayQuestion();
        resultPanel.SetActive(false);

        remainingTime = totalQuizTime;
        quizOngoing = true;
    }

    void Update()
    {
        if (!quizOngoing) return;

        remainingTime -= Time.deltaTime;
        timerText.text = Mathf.Ceil(remainingTime).ToString("0");

        if (remainingTime <= 0)
        {
            EndQuiz();
        }
    }

   

    public void EndQuiz()
    {
        if (!quizOngoing) return;
        quizOngoing = false;

        if (circularTimer != null)
        {
            circularTimer.StopTimer();
        }
        ShowResultPopup();
    }

    void ShowResultPopup()
    {
        resultPanel.SetActive(true);
        resultPointText.text = "Poinmu: " + score;

        RewardUIController reward = FindFirstObjectByType<RewardUIController>();
        if (reward != null)
        {
            reward.AddPoints(score);
        }
        StartCoroutine(SendPointsToDatabase(score));
    }

    IEnumerator SendPointsToDatabase(int score)
    {
        string username = PlayerPrefs.GetString("LoggedInUsername", "UnknownUser");

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("points", score);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/LoginFolder/UpdatePoints.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Points updated to DB: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to send points: " + www.error);
            }
        }
    }


    void LoadQuestions()
    {
        questions = new List<Question>()
        {
            new Question
            {
                question = "Mana yang paling kaya vitamin C?",
                options = new string[] { "Pisang", "Jeruk", "Apel", "Mangga" },
                correctAnswerIndex = 1
            },
            new Question
            {
                question = "Buah apa yang bisa membantu pencernaan karena mengandung enzim bromelain?",
                options = new string[] { "Nanas", "Semangka", "Apel", "Stroberi" },
                correctAnswerIndex = 0
            },
            new Question
            {
                question = "Buah yang bisa membantu menurunkan tekanan darah?",
                options = new string[] { "Durian", "Pisang", "Anggur", "Melon" },
                correctAnswerIndex = 1
            },
            new Question
            {
                question = "Jus sayuran yang baik diminum pagi hari adalah",
                options = new string[] { "Jus kentang", "Jus sawi", "Jus wortel", "Jus terong" },
                correctAnswerIndex = 2
            },
            new Question
            {
                question = "Apa manfaat utama serat dari buah dan sayur?",
                options = new string[] { "Menguatkan tulang", "Mencerahkan kulit", "Melancarkan pencernaan", "Menambah gula darah" },
                correctAnswerIndex = 2
            },
            new Question
            {
                question = "Mengonsumsi buah dan sayur dapat menurunkan risiko penyakit...",
                options = new string[] { "Flu biasa", "Diabetes, stroke, dan kanker", "Sakit kepala ringan", "Demam" },
                correctAnswerIndex = 1
            },
            new Question
            {
                question = "Berapa porsi buah & sayur sebaiknya dikonsumsi setiap hari menurut WHO?",
                options = new string[] { "1–2 porsi", "3 porsi", "5 porsi", "Hanya saat sakit" },
                correctAnswerIndex = 2
            },
            new Question
            {
                question = "Apa yang terjadi jika kamu kekurangan serat dari buah dan sayur?",
                options = new string[] { "Berat badan cepat turun", "Sulit buang air besar dan mudah lelah", "Mata jadi rabun", "Gigi cepat copot" },
                correctAnswerIndex = 1
            },
            new Question
            {
                question = "Sayur yang sering disebut sebagai “superfood” karena antioksidan tinggi",
                options = new string[] { "Selada", "Kangkung", "Brokoli", "Buncis" },
                correctAnswerIndex = 2
            },
            new Question
            {
                question = "Kamu hanya bisa makan 1 buah hari ini. Pilihan paling lengkap nutrisinya adalah",
                options = new string[] { "Apel", "Jeruk", "Alpukat", "Semangka" },
                correctAnswerIndex = 2
            }

        };
    }

    void DisplayQuestion()
    {
        questionNumberText.text = $"Question {currentQuestionIndex + 1}/{questions.Count}";

        if (currentQuestionIndex >= questions.Count)
        {
            Debug.Log("Kuis selesai!");
            return;
        }

        Question current = questions[currentQuestionIndex];
        questionText.text = current.question;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            optionTexts[i].text = current.options[i];

            Button btn = optionTexts[i].transform.parent.GetComponent<Button>();
            Image btnImage = btn.GetComponent<Image>();

            btn.interactable = true;
            btnImage.color = Color.white;

            btn.onClick.RemoveAllListeners();
            int index = i;
            btn.onClick.AddListener(() => CheckAnswer(index));

            if (resultIcons.Length > i && resultIcons[i] != null)
            {
                resultIcons[i].enabled = false;
            }
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        int correctIndex = questions[currentQuestionIndex].correctAnswerIndex;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            Button btn = optionTexts[i].transform.parent.GetComponent<Button>();
            Image btnImage = btn.GetComponent<Image>();

            if (i == correctIndex)
                btnImage.color = Color.green;
            else if (i == selectedIndex)
                btnImage.color = Color.red;

            btn.interactable = false;
        }

        if (selectedIndex == correctIndex)
        {
            score += pointPerCorrect;
        }

        Invoke("NextQuestion", 1.5f); 
    }


    void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Count)
        {
            DisplayQuestion();
        }
        else
        {
            EndQuiz(); 
        }
    }

}


[System.Serializable]
public class Question
{
    public string question;
    public string[] options;
    public int correctAnswerIndex;
}
