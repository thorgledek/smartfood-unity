using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircularTimer : MonoBehaviour
{
    public Image circleFillImage;
    public TMP_Text timerText;
    public float totalTime = 60f;

    private float timeLeft;
    private bool isRunning = true;

    void Start()
    {
        timeLeft = totalTime;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) timeLeft = 0;

        // Update lingkaran
        float fillAmount = timeLeft / totalTime;
        circleFillImage.fillAmount = fillAmount;

        // Update text waktu
        if (timerText != null)
            timerText.text = Mathf.Ceil(timeLeft).ToString("0");
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    // Optional: bisa dipakai ulang jika timer ingin di-reset
    public void ResetTimer(float newTime)
    {
        totalTime = newTime;
        timeLeft = newTime;
        isRunning = true;
    }
}
