using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflejo : MonoBehaviour
{
    int maxBounces = 5;  // N�mero m�ximo de rebotes del l�ser
    private LineRenderer lr;
    [SerializeField]
    private Transform startPoint;  // Punto de inicio del l�ser
    [SerializeField]
    private bool reflectOnlyMirror;  // Indica si solo debe reflejar en objetos con el tag "Mirror"
    [SerializeField]
    private Color laserColor = Color.red;  // Color del l�ser, por defecto rojo

    private GameObject botonObjeto;  
    private BotonInteractivo botonScript;  
    private bool tocandoBoton;  

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = maxBounces + 1;  // Configura el n�mero de posiciones del LineRenderer
        lr.SetPosition(0, startPoint.position);

        // Cambiar el color del material del LineRenderer
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = laserColor;

        // Buscar el objeto con el tag "boton" y obtener su script BotonInteractivo
        botonObjeto = GameObject.FindGameObjectWithTag("boton");
        if (botonObjeto != null)
        {
            botonScript = botonObjeto.GetComponent<BotonInteractivo>();
        }

        tocandoBoton = false;  // Inicializar la variable tocandoBoton
    }

    void Update()
    {
        // Castear el l�ser cada frame
        CastLaser(startPoint.position, -startPoint.forward);

        // Restaurar color original del bot�n si ya no est� tocando
        if (!tocandoBoton && botonScript != null)
        {
            botonScript.RestaurarColorOriginal();  // Asumiendo que has implementado este m�todo en BotonInteractivo.cs
            botonScript.ActivarDesactivarTrigger(false);
        }

        tocandoBoton = false;  // Reiniciar la variable tocandoBoton al inicio de cada frame
    }

    void CastLaser(Vector3 position, Vector3 direction)
    {
        lr.positionCount = 1;  // Reinicia el n�mero de posiciones a 1 para empezar de nuevo
        lr.SetPosition(0, startPoint.position);

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300))
            {
                position = hit.point;

                // Verificar si el l�ser toca el objeto con el tag "boton"
                if (hit.transform.CompareTag("boton") && botonScript != null)
                {
                    tocandoBoton = true;

                    // Cambiar color del bot�n y activar el trigger del objeto asociado
                    botonScript.CambiarColorActivo();
                    botonScript.ActivarDesactivarTrigger(true);
                }

                if (hit.transform.CompareTag("Mirror") || !reflectOnlyMirror)
                {
                    direction = Vector3.Reflect(direction, hit.normal);
                    lr.positionCount = i + 2;  // Incrementa el n�mero de posiciones seg�n los rebotes
                    lr.SetPosition(i + 1, hit.point);
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
                // Si no golpea nada, termina el bucle y coloca el �ltimo punto
                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, position + direction * 300);
                break;
            }
        }
    }
}

