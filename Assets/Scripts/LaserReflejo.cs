using UnityEngine;

public class LaserReflejo : MonoBehaviour
{
    int maxBounces = 5;  // Número máximo de rebotes del láser
    private LineRenderer lr;
    [SerializeField]
    private Transform startPoint;  // Punto de inicio del láser
    [SerializeField]
    private bool reflectOnlyMirror;  // Indica si solo debe reflejar en objetos con el tag "Mirror"
    [SerializeField]
    private Color laserColor = Color.red;  // Color del láser, por defecto rojo

    private GameObject lastButtonHit;  // Último objeto con el que colisionó que tenía el tag "boton"

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = maxBounces + 1;  // Configura el número de posiciones del LineRenderer
        lr.SetPosition(0, startPoint.position);

        // Cambiar el color del material del LineRenderer
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = laserColor;
    }

    void Update()
    {
        CastLaser(startPoint.position, -startPoint.forward);
    }

    void CastLaser(Vector3 position, Vector3 direction)
    {
        lr.positionCount = 1;  // Reinicia el número de posiciones a 1 para empezar de nuevo
        lr.SetPosition(0, startPoint.position);

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300))
            {
                position = hit.point;

                if (hit.transform.CompareTag("Mirror") || !reflectOnlyMirror)
                {
                    direction = Vector3.Reflect(direction, hit.normal);
                    lr.positionCount = i + 2;  // Incrementa el número de posiciones según los rebotes
                    lr.SetPosition(i + 1, hit.point);

                    // Si el objeto tocado tiene el tag "boton"
                    if (hit.transform.CompareTag("boton"))
                    {
                        // Guarda una referencia al objeto boton
                        lastButtonHit = hit.transform.gameObject;

                        // Cambia el color y activa el trigger del otro objeto
                        CambiarColorActivarTriggerOtroObjeto(true);
                    }
                }
                else
                {
                    // Si no tiene el tag "Mirror" y se requiere reflejar solo en "Mirror"
                    lr.positionCount = i + 2;
                    lr.SetPosition(i + 1, hit.point);
                    break;
                }
            }
            else
            {
                // Si no golpea nada, termina el bucle y coloca el último punto
                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, position + direction * 300);
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si entra en contacto con un objeto con tag "boton"
        if (other.CompareTag("boton"))
        {
            lastButtonHit = other.gameObject;

            // Cambia el color y activa el trigger del otro objeto
            CambiarColorActivarTriggerOtroObjeto(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si sale del contacto con el objeto con tag "boton"
        if (other.CompareTag("boton") && other.gameObject == lastButtonHit)
        {
            // Revierte los cambios (vuelve al color original y desactiva el trigger)
            CambiarColorActivarTriggerOtroObjeto(false);
            lastButtonHit = null;
        }
    }

    void CambiarColorActivarTriggerOtroObjeto(bool activate)
    {
        // Encuentra el otro objeto que deseas cambiar y activar su trigger
        // Aquí debes especificar cómo obtienes o encuentras ese otro objeto
        // Supongamos que el objeto se encuentra a través de una referencia directa en el script
        GameObject otroObjeto = GameObject.Find("NombreDelOtroObjeto"); // Cambia por el nombre correcto o referencia directa

        if (otroObjeto != null)
        {
            Renderer rend = otroObjeto.GetComponent<Renderer>();
            if (rend != null)
            {
                // Cambia el color del material del otro objeto
                rend.material.color = activate ? Color.green : Color.white;

                // Activa o desactiva el trigger del otro objeto (asumiendo que tiene un componente Collider con IsTrigger)
                Collider triggerCollider = otroObjeto.GetComponent<Collider>();
                if (triggerCollider != null)
                {
                    triggerCollider.isTrigger = activate;
                }
            }
        }
    }
}
