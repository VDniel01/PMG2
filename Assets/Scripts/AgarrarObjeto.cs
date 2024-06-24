using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgarrarObjeto : MonoBehaviour
{
    public GameObject handPoint;
    public Image miraImage; 
    public Color colorEnRango = Color.green; 
    private Color colorOriginal; 

    private GameObject pickedObject = null;
    private Collider pickedObjectCollider = null;
    private bool isInRange = false; 

    void Start()
    {
        if (miraImage != null)
        {
            colorOriginal = miraImage.color;
        }
    }

    void Update()
    {
        if (pickedObject != null)
        {
            if (Input.GetMouseButton(0)) 
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
            
            isInRange = true;

            if (Input.GetMouseButton(0) && pickedObject == null) 
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
          
            isInRange = false;
        }
    }
}
