using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickup : MonoBehaviour
{
    public float timeToAdd = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CountdownTimer countdownTimer = FindObjectOfType<CountdownTimer>();
            if (countdownTimer != null)
            {
                countdownTimer.AddTime(timeToAdd);
                Destroy(gameObject); 
            }
            else
            {
               
            }
        }
    }
}
