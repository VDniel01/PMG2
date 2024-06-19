using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public GameObject objetoAActivar;  // Objeto que se activará/desactivará
    public Color colorActivo = Color.green;  // Color cuando el botón está activo
    private Color colorOriginal;  // Color original del botón

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        colorOriginal = rend.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            // Cambiar color y activar el objeto
            rend.material.color = colorActivo;

            if (objetoAActivar != null)
            {
                ActivarDesactivarTrigger(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            // Revertir color y desactivar el objeto
            rend.material.color = colorOriginal;

            if (objetoAActivar != null)
            {
                ActivarDesactivarTrigger(false);
            }
        }
    }

    void ActivarDesactivarTrigger(bool activar)
    {
        // Activar o desactivar el trigger del objeto especificado
        Collider triggerCollider = objetoAActivar.GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = activar;
        }
    }
}
