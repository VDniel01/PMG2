using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = maxBounces + 1;  // Configura el número de posiciones del LineRenderer
        lr.SetPosition(0, startPoint.position);

        // Cambiar el color del material del LineRenderer
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = laserColor;
    }

    // Update is called once per frame
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
}
