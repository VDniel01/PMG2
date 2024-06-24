using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 100f;
    public LayerMask groundMask;
    public float maxHealth = 100f;
    public float currentHealth;
    public Canvas gameOverCanvas;
    public float laserDamagePerSecond = 10f; // daño por segundo del láser

    private Rigidbody rb;
    private Transform cameraTransform;
    private float xRotation = 0f;
    private bool isGrounded;
    private Vector3 groundNormal;
    private bool isGameOver = false;

    private Vector3 initialPosition; 
    private Quaternion initialRotation; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        currentHealth = maxHealth;

        // guardar la posición y rotación inicial del jugador
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


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
            HandleJump();
            ReduceHealthOverTime();
            CheckHealthPickup();
        }
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        Vector3 velocity = move * speed;

        if (isGrounded)
        {
            rb.velocity = Vector3.ProjectOnPlane(velocity, groundNormal) + Vector3.up * rb.velocity.y;
        }
        else
        {
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
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

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 
        }
    }

    void FixedUpdate()
    {
        CheckGround();
    }

    void CheckGround()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.3f, groundMask))
        {
            isGrounded = true;
            groundNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up; 
        }
    }

    void ReduceHealthOverTime()
    {
        if (isGrounded)
        {
            TakeDamage(laserDamagePerSecond * Time.deltaTime); // aplica daño por segundo
        }
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
        Time.timeScale = 0; // pausa el juego y activa el game over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reanuda el juego
        ResetPlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ResetPlayer()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        currentHealth = maxHealth;

        isGameOver = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    void CheckHealthPickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        HealthPickup pickupComponent = healthPickup.GetComponent<HealthPickup>();
        if (pickupComponent != null)
        {
            float healthToAdd = pickupComponent.healthToRecover;
            Destroy(healthPickup); 
            currentHealth += healthToAdd;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }
}
