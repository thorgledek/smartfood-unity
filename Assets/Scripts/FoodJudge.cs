using UnityEngine;
using UnityEngine.UI;

public class FoodJudge : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    public void AddScore(bool correct)
    {
        if (correct)
        {
            score++;
            Debug.Log("Benar!");
        }
        else
        {
            Debug.Log("Salah!");
        }

        scoreText.text = "Skor: " + score;
    }
}
