using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflejo : MonoBehaviour
{
    int maxBounces = 5;
    private LineRenderer lr;
    [SerializeField]
    private Transform startPoint;  // punto de inicio del láser
    [SerializeField]
    private bool reflectOnlyMirror;  // Indica si debe reflejar en objetos con el tag Mirror
    [SerializeField]
    private Color laserColor = Color.red;  
    public float damagePerSecond = 10f; // daño por segundo del laser

    private List<GameObject> botonesObjetos;
    private List<BotonInteractivo> botonesScripts;
    private List<bool> tocandoBotones;
    private PlayerMovement playerMovement;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = maxBounces + 1;  
        lr.SetPosition(0, startPoint.position);

 
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = laserColor;

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

        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        CastLaser(startPoint.position, -startPoint.forward);

        for (int i = 0; i < tocandoBotones.Count; i++)
        {
            if (!tocandoBotones[i] && botonesScripts[i] != null)
            {
                botonesScripts[i].RestaurarColorOriginal();
                botonesScripts[i].ActivarDesactivarTrigger(false);
            }
            tocandoBotones[i] = false;
        }
    }

    void CastLaser(Vector3 position, Vector3 direction)
    {
        lr.positionCount = 1; 
        lr.SetPosition(0, startPoint.position);

        for (int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300))
            {
                position = hit.point;

                // verifica si el laser toca algun objeto con el tag boton
                for (int j = 0; j < botonesObjetos.Count; j++)
                {
                    if (hit.transform == botonesObjetos[j].transform && botonesScripts[j] != null)
                    {
                        tocandoBotones[j] = true;

                        botonesScripts[j].CambiarColorActivo();
                        botonesScripts[j].ActivarDesactivarTrigger(true);
                    }
                }

                // verifica si el laser toca al jugador para hacerle daño
                if (hit.transform.CompareTag("Player") && playerMovement != null)
                {
                    playerMovement.TakeDamage(damagePerSecond * Time.deltaTime);
                }

                if (hit.transform.CompareTag("Mirror") || !reflectOnlyMirror)
                {
                    direction = Vector3.Reflect(direction, hit.normal);
                    lr.positionCount = i + 2;
                    lr.SetPosition(i + 1, hit.point);
                }
                else
                {
                    lr.positionCount = i + 2;
                    lr.SetPosition(i + 1, hit.point);
                    break;
                }
            }
            else
            {
                lr.positionCount = i + 2;
                lr.SetPosition(i + 1, position + direction * 300);
                break;
            }
        }
    }
}
