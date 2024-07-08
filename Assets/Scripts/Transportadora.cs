using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transportadora : MonoBehaviour
{
    public float speed = 2.0f;
    public Vector3 direction = Vector3.forward;

    private List<Rigidbody> objectsOnBelt = new List<Rigidbody>();

    void FixedUpdate()
    {
        foreach (Rigidbody rb in objectsOnBelt)
        {
            Vector3 movement = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null && !objectsOnBelt.Contains(rb))
        {
            objectsOnBelt.Add(rb);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null && objectsOnBelt.Contains(rb))
        {
            rb.velocity = Vector3.zero; // Detener el objeto al salir de la transportadora
            objectsOnBelt.Remove(rb);
        }
    }
}