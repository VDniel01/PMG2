using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transportadora : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad de la cinta transportadora
    public Vector3 direction = Vector3.forward; // Dirección en la que se moverán los objetos

    private List<Rigidbody> objectsOnBelt = new List<Rigidbody>();

    void FixedUpdate()
    {
        // Mover todos los objetos en la cinta transportadora
        foreach (Rigidbody rb in objectsOnBelt)
        {
            Vector3 movement = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            objectsOnBelt.Add(rb);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            objectsOnBelt.Remove(rb);
        }
    }
}
