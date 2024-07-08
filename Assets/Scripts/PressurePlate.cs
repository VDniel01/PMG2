using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameObject puerta;  // La puerta que será activada por esta placa
    [SerializeField]
    private Color colorActivo = Color.green;  // Color de la puerta cuando está activa
    private Color colorOriginal;  // Color original de la puerta

    private Renderer puertaRenderer;  // Renderer de la puerta
    private Collider puertaCollider;  // Collider de la puerta

    void Start()
    {
        if (puerta != null)
        {
            puertaRenderer = puerta.GetComponent<Renderer>();
            puertaCollider = puerta.GetComponent<Collider>();
            if (puertaRenderer != null)
            {
                colorOriginal = puertaRenderer.material.color;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && puerta != null)
        {
            CambiarEstadoPuerta(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && puerta != null)
        {
            CambiarEstadoPuerta(false);
        }
    }

    private void CambiarEstadoPuerta(bool activar)
    {
        if (puertaRenderer != null)
        {
            puertaRenderer.material.color = activar ? colorActivo : colorOriginal;
        }

        if (puertaCollider != null)
        {
            puertaCollider.isTrigger = activar;
        }
    }
}
