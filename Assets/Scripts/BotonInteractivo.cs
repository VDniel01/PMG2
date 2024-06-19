using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public GameObject objetoAActivar;  // Objeto cuyo trigger se activar�/desactivar�
    public Color colorActivo = Color.green;  // Color cuando el bot�n est� activo
    private Color colorOriginal;  // Color original del bot�n

    private Renderer rend;
    private Renderer objetoRenderer;  // Renderer del objeto asociado al bot�n

    void Start()
    {
        rend = GetComponent<Renderer>();
        colorOriginal = rend.material.color;

        // Obtener el Renderer del objeto asociado al bot�n
        if (objetoAActivar != null)
        {
            objetoRenderer = objetoAActivar.GetComponent<Renderer>();
        }
    }

    public void CambiarColorActivo()
    {
        // Cambiar color del bot�n
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

        // Cambiar color del objeto asociado al bot�n seg�n est� activo o no el trigger
        if (objetoRenderer != null)
        {
            objetoRenderer.material.color = activar ? colorActivo : colorOriginal;
        }
    }

    public void RestaurarColorOriginal()
    {
        // Restaurar el color original del bot�n
        rend.material.color = colorOriginal;
    }
}

