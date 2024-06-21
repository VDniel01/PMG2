using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgarrarObjeto : MonoBehaviour
{
    public GameObject handPoint;
    public Image miraImage; // Referencia a la imagen de la mira
    public Color colorEnRango = Color.green; // Color cuando está en rango para agarrar objeto
    private Color colorOriginal; // Color original de la mira

    private GameObject pickedObject = null;
    private Collider pickedObjectCollider = null;
    private bool isInRange = false; // Indica si está en rango para agarrar objeto

    void Start()
    {
        // Guardar el color original de la mira
        if (miraImage != null)
        {
            colorOriginal = miraImage.color;
        }
    }

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

        // Cambiar color de la mira cuando está en rango
        if (miraImage != null)
        {
            if (isInRange)
            {
                miraImage.color = colorEnRango;
            }
            else
            {
                miraImage.color = colorOriginal;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mirror"))
        {
            // Cambiar el estado de rango
            isInRange = true;

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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mirror"))
        {
            // Cambiar el estado de rango
            isInRange = false;
        }
    }
}
