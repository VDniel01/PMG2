using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public List<GameObject> objetosAActivar;  // Lista de objetos cuyos triggers se activar�n/desactivar�n
    public Color colorActivo = Color.green;  // Color cuando el bot�n est� activo
    private Color colorOriginal;  // Color original del bot�n

    private Renderer rend;
    private List<Renderer> objetoRenderers;  // Lista de Renderers de los objetos asociados al bot�n

    void Start()
    {
        rend = GetComponent<Renderer>();
        colorOriginal = rend.material.color;

        objetoRenderers = new List<Renderer>();

        // Obtener los Renderers de los objetos asociados al bot�n
        foreach (GameObject objeto in objetosAActivar)
        {
            if (objeto != null)
            {
                Renderer objRend = objeto.GetComponent<Renderer>();
                if (objRend != null)
                {
                    objetoRenderers.Add(objRend);
                }
            }
        }
    }

    public void CambiarColorActivo()
    {
        // Cambiar color del bot�n
        rend.material.color = colorActivo;
    }

    public void ActivarDesactivarTrigger(bool activar)
    {
        // Activar o desactivar el trigger de los objetos especificados
        foreach (GameObject objeto in objetosAActivar)
        {
            Collider triggerCollider = objeto.GetComponent<Collider>();
            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = activar;
            }
        }

        // Cambiar color de los objetos asociados al bot�n seg�n est�n activos o no los triggers
        foreach (Renderer objRend in objetoRenderers)
        {
            if (objRend != null)
            {
                objRend.material.color = activar ? colorActivo : colorOriginal;
            }
        }
    }

    public void RestaurarColorOriginal()
    {
        // Restaurar el color original del bot�n
        rend.material.color = colorOriginal;

        // Restaurar el color original de los objetos asociados al bot�n
        foreach (Renderer objRend in objetoRenderers)
        {
            if (objRend != null)
            {
                objRend.material.color = colorOriginal;
            }
        }
    }
}
