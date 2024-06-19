using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public GameObject objetoAActivar;  // Objeto cuyo trigger se activará/desactivará
    public Color colorActivo = Color.green;  // Color cuando el botón está activo
    private Color colorOriginal;  // Color original del botón

    private Renderer rend;
    private Renderer objetoRenderer;  // Renderer del objeto asociado al botón

    void Start()
    {
        rend = GetComponent<Renderer>();
        colorOriginal = rend.material.color;

        // Obtener el Renderer del objeto asociado al botón
        if (objetoAActivar != null)
        {
            objetoRenderer = objetoAActivar.GetComponent<Renderer>();
        }
    }

    public void CambiarColorActivo()
    {
        // Cambiar color del botón
        rend.material.color = colorActivo;
    }

    public void ActivarDesactivarTrigger(bool activar)
    {
        // Activar o desactivar el trigger del objeto especificado
        Collider triggerCollider = objetoAActivar.GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = activar;
        }

        // Cambiar color del objeto asociado al botón según esté activo o no el trigger
        if (objetoRenderer != null)
        {
            objetoRenderer.material.color = activar ? colorActivo : colorOriginal;
        }
    }

    public void RestaurarColorOriginal()
    {
        // Restaurar el color original del botón
        rend.material.color = colorOriginal;
    }
}

