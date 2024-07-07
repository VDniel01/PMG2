using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public List<GameObject> objetosAActivar;
    public Color colorActivo = Color.green;
    private Color colorOriginal;

    private Renderer rend;
    private List<Renderer> objetoRenderers;

    void Start()
    {
        rend = GetComponent<Renderer>();
        colorOriginal = rend.material.color;

        objetoRenderers = new List<Renderer>();

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
        rend.material.color = colorActivo;
    }

    public void ActivarDesactivarTrigger(bool activar)
    {
        foreach (GameObject objeto in objetosAActivar)
        {
            Collider triggerCollider = objeto.GetComponent<Collider>();
            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = activar;
            }
        }

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
        rend.material.color = colorOriginal;
        foreach (Renderer objRend in objetoRenderers)
        {
            if (objRend != null)
            {
                objRend.material.color = colorOriginal;
            }
        }
    }
}
