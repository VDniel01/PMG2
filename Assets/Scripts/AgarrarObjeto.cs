using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgarrarObjeto : MonoBehaviour
{
    public GameObject handPoint;

    private GameObject pickedObject = null;
    private Collider pickedObjectCollider = null;

    void Update()
    {
        if (pickedObject != null)
        {
            if (Input.GetMouseButton(0)) // Botón izquierdo del mouse
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = false;
                pickedObject.GetComponent<Rigidbody>().isKinematic = true;
                pickedObject.transform.position = handPoint.transform.position;
                pickedObject.transform.SetParent(handPoint.transform);
            }
            else
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.transform.SetParent(null);
                if (pickedObjectCollider != null)
                {
                    pickedObjectCollider.enabled = true;
                }
                pickedObject = null;
                pickedObjectCollider = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mirror"))
        {
            if (Input.GetMouseButton(0) && pickedObject == null) // Botón izquierdo del mouse
            {
                pickedObject = other.gameObject;
                pickedObjectCollider = pickedObject.GetComponent<Collider>();
                if (pickedObjectCollider != null)
                {
                    pickedObjectCollider.enabled = false;
                }
            }
        }
    }
}
