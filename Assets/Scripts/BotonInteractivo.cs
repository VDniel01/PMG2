using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public GameObject objetoAActivar;  // Objeto cuyo trigger se activará/desactivará
    public Color colorActivo = Color.green;  // Color cuando el botón está activo
    private Color colorOriginal;  // Color original del botón

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
        // Restablecer el color original del botón
        rend.material.color = colorOriginal;
    }
}

