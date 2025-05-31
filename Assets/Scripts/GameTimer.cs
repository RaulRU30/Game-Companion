using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float startTime = 10f; // Tiempo inicial en segundos
    private float timeRemaining;
    private TextMeshProUGUI timerText;
    private bool isRunning = false;
    

    void Start()
    {
        timeRemaining = startTime;
        timerText = GetComponent<TextMeshProUGUI>();
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0);
            UpdateTimerDisplay();

            if (timeRemaining <= 0)
            {
                isRunning = false;
                OnTimerEnd();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartCountdown()
    {
        timeRemaining = startTime;
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        timeRemaining = startTime;
        UpdateTimerDisplay();
    }
    public TaskIndicator[] taskIndicators;
    void OnTimerEnd() // esta es la funcion donde acaba el timer
    {
        timerText.text = "GAME OVER";
        Debug.Log("Se acabo el tiempo");
        //aqui es una prueba
        foreach (TaskIndicator ti in taskIndicators)
        {
            ti.MarcarCompletado();
        }

        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(2f); // Pequeño delay visual
        SceneManager.LoadScene("GameOver"); // Usa el nombre exacto de la escena
    }
}
