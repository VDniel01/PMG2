using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflejo : MonoBehaviour
{
    int maxBounces = 5;
    private LineRenderer lr;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private bool reflectOnlyMirror;
    [SerializeField]
    private Color laserColor = Color.red;
    public float damagePerSecond = 10f;

    private List<BotonInteractivo> botonesScripts;
    private PlayerMovement playerMovement;

    // Listas para rastrear los botones tocados
    private List<BotonInteractivo> botonesTocados = new List<BotonInteractivo>();
    private List<BotonInteractivo> botonesTocadosPrev = new List<BotonInteractivo>();

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = maxBounces + 1;
        lr.SetPosition(0, startPoint.position);

        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = laserColor;

        GameObject[] botonesObjetos = GameObject.FindGameObjectsWithTag("boton");
        botonesScripts = new List<BotonInteractivo>();

        foreach (GameObject boton in botonesObjetos)
        {
            BotonInteractivo botonScript = boton.GetComponent<BotonInteractivo>();
            if (botonScript != null)
            {
                botonesScripts.Add(botonScript);
            }
        }

        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        botonesTocadosPrev = new List<BotonInteractivo>(botonesTocados);
        botonesTocados.Clear();

        CastLaser(startPoint.position, -startPoint.forward);

        foreach (BotonInteractivo boton in botonesTocadosPrev)
        {
            if (!botonesTocados.Contains(boton))
            {
                boton.RestaurarColorOriginal();
                boton.ActivarDesactivarTrigger(false);
            }
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

                if (hit.transform.CompareTag("boton"))
                {
                    BotonInteractivo botonScript = hit.transform.GetComponent<BotonInteractivo>();
                    if (botonScript != null)
                    {
                        botonScript.CambiarColorActivo();
                        botonScript.ActivarDesactivarTrigger(true);
                        botonesTocados.Add(botonScript);
                    }
                }

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