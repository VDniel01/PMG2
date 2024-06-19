using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public GameObject objetoAActivar;  // Objeto cuyo trigger se activar�/desactivar�
    public Color colorActivo = Color.green;  // Color cuando el bot�n est� activo
    private Color colorOriginal;  // Color original del bot�n

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        colorOriginal = rend.material.color;
    }

    public void CambiarColorActivo()
    {
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
    }

    public void RestaurarColorOriginal()
    {
        // Restablecer el color original del bot�n
        rend.material.color = colorOriginal;
    }
}

