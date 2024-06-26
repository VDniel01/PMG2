using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraDeVida;
    public PlayerMovement playerMovement;

    void Update()
    {
        if (playerMovement != null)
        {
            barraDeVida.fillAmount = playerMovement.currentHealth / playerMovement.maxHealth;
        }
    }
}
