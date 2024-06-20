using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflejo : MonoBehaviour
{
    int maxBounces = 5;
    private LineRenderer lr;
    [SerializeField]
    private Transform startPoint;  // Punto de inicio del láser
    [SerializeField]
    private bool reflectOnlyMirror;  // Indica si solo debe reflejar en objetos con el tag "Mirror"
    [SerializeField]
    private Color laserColor = Color.red;  // Color del láser, por defecto rojo

    private List<GameObject> botonesObjetos;
    private List<BotonInteractivo> botonesScripts;
    private List<bool> tocandoBotones;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = maxBounces + 1;  // Configura el número de posiciones del LineRenderer
        lr.SetPosition(0, startPoint.position);

        // Cambiar el color del material del LineRenderer
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = laserColor;

        // Buscar todos los objetos con el tag "boton" y obtener sus scripts BotonInteractivo
        botonesObjetos = new List<GameObject>(GameObject.FindGameObjectsWithTag("boton"));
        botonesScripts = new List<BotonInteractivo>();
        tocandoBotones = new List<bool>();

        foreach (GameObject boton in botonesObjetos)
        {
            BotonInteractivo botonScript = boton.GetComponent<BotonInteractivo>();
            if (botonScript != null)
            {
                botonesScripts.Add(botonScript);
                tocandoBotones.Add(false);
            }
        }
    }

    void Update()
    {
        // Castear el láser cada frame
        CastLaser(startPoint.position, -startPoint.forward);

        // Restaurar color original de los botones si ya no están tocando
        for (int i = 0; i < tocandoBotones.Count; i++)
        {
            if (!tocandoBotones[i] && botonesScripts[i] != null)
            {
                botonesScripts[i].RestaurarColorOriginal();
                botonesScripts[i].ActivarDesactivarTrigger(false);
            }
            tocandoBotones[i] = false;  // Reiniciar la variable tocandoBoton al inicio de cada frame
        }
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

                // Verificar si el láser toca algún objeto con el tag "boton"
                for (int j = 0; j < botonesObjetos.Count; j++)
                {
                    if (hit.transform == botonesObjetos[j].transform && botonesScripts[j] != null)
                    {
                        tocandoBotones[j] = true;

                        // Cambiar color del botón y activar el trigger del objeto asociado
                        botonesScripts[j].CambiarColorActivo();
                        botonesScripts[j].ActivarDesactivarTrigger(true);
                    }
                }

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
