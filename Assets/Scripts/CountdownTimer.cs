using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 60f; // Tiempo inicial en segundos
    private float currentTime;
    public Text timerText;
    public string nextSceneName = "Level2"; // Nombre de la próxima escena
    public Canvas gameOverCanvas;
    private bool isGameOver = false;
    private bool isTimerActive = false; // Controla si el temporizador está activo

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();

        // Desactivar el Canvas de Game Over al iniciar
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
        timerText.text = currentTime.ToString("F2"); // Formatear con 2 decimales
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // Pausar el juego
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
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene(nextSceneName); // Cargar la siguiente escena
    }

    public void StartTimer()
    {
        isTimerActive = true; // Activar el temporizador
    }
}
