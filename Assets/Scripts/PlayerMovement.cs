using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public float maxHealth = 100f;
    public float currentHealth;
    public Canvas gameOverCanvas;
    public float laserDamagePerSecond = 10f; // Daño por segundo del láser

    private Transform cameraTransform;
    private float xRotation = 0f;
    private bool isGameOver = false;

    private Vector3 initialPosition; // Posición inicial del jugador
    private Quaternion initialRotation; // Rotación inicial del jugador

    void Start()
    {
        cameraTransform = Camera.main.transform;
        currentHealth = maxHealth;

        // Guardar la posición y rotación inicial del jugador
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Desactivar el Canvas de Game Over al iniciar
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            HandleMovement();
            HandleMouseLook();
            CheckHealthPickup();
        }
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        Vector3 velocity = move * speed;

        GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, GetComponent<Rigidbody>().velocity.y, velocity.z);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // Pausar el juego
        // Mostrar Game Over y botón para reiniciar
        Debug.Log("Game Over!");

        // Activar el Canvas de Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Resumir el juego
        ResetPlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recargar la escena actual
    }

    void ResetPlayer()
    {
        // Reiniciar la posición y rotación del jugador
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reiniciar la salud
        currentHealth = maxHealth;

        // Reiniciar el estado del juego
        isGameOver = false;

        // Ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Desactivar el Canvas de Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    void CheckHealthPickup()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Cambia la tecla según necesites
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
            {
                if (hit.collider.CompareTag("recuperarHP"))
                {
                    RecuperarSalud(hit.collider.gameObject);
                }
            }
        }
    }

    void RecuperarSalud(GameObject healthPickup)
    {
        // Obtener el componente HealthPickup para obtener la cantidad de salud a recuperar
        HealthPickup pickupComponent = healthPickup.GetComponent<HealthPickup>();
        if (pickupComponent != null)
        {
            float healthToAdd = pickupComponent.healthToRecover;
            Destroy(healthPickup); // Destruir el objeto recuperador de salud

            // Añadir salud al jugador
            currentHealth += healthToAdd;

            // Asegurar que la salud no exceda el máximo
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("recuperarHP"))
        {
            RecuperarSalud(other.gameObject);
        }
    }
}
