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
                question = "Berapa banyak kalori yang dibutuhkan manusia dalam sehari?",
                options = new string[] { "2500 kalori per hari", "25.000 kalori per hari", "250 kalori per hari", "25 kalori per hari" },
                correctAnswerIndex = 0
            },
            new Question
            {
                question = "Apa ibu kota Indonesia?",
                options = new string[] { "Surabaya", "Bandung", "Jakarta", "Medan" },
                correctAnswerIndex = 2
            },
            new Question
            {
                question = "Siapa penemu lampu pijar?",
                options = new string[] { "Einstein", "Newton", "Edison", "Tesla" },
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
