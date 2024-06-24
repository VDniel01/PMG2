using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 60f; // tiempo inicial
    private float currentTime;
    public Text timerText;
    public string nextSceneName = "Level2"; 
    public Canvas gameOverCanvas;
    private bool isGameOver = false;
    private bool isTimerActive = false; 

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();

        
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isTimerActive && !isGameOver)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();

            if (currentTime <= 0)
            {
                currentTime = 0;
                GameOver();
            }
        }
    }

    void UpdateTimerText()
    {
        timerText.text = currentTime.ToString("F2"); 
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // pausar el juego
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void AddTime(float amount)
    {
        currentTime += amount;
        UpdateTimerText(); 
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene(nextSceneName); // cargar la siguiente escena
    }

    public void StartTimer()
    {
        isTimerActive = true; // activa el temporizador
    }
}
