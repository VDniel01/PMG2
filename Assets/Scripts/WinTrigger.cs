using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public Canvas winCanvas;

    private void Start()
    {
        if (winCanvas != null)
        {
            winCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (winCanvas != null)
            {
                winCanvas.gameObject.SetActive(true);
                Time.timeScale = 0f; 
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene("Menu"); 
    }
}
