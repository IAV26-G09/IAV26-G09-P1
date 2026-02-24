using System;
using UCM.IAV.Movimiento;
using UnityEngine;

/// <summary>
/// Clase para modelar el comportamiento de PERSEGUIR a otro agente
/// </summary>
public class Persecucion : Llegada
{
    [SerializeField]
    private float maxPrediction;

    [SerializeField]
    private GameObject objetivoReal; // aqui le pasas el flautista, en el objetivo del agente se mete un objetivo vacio a predecir en cada update

    [SerializeField] private bool debugPrediction = false;

    private void Start()
    {
        if (objetivoReal== null)
        {
            objetivoReal = new GameObject();
        }
    }

    public override ComportamientoDireccion GetComportamientoDireccion()
    {
        // direccion hacia el objetivo
        Vector3 direccion = objetivo.transform.position - agente.transform.position;
        // distancia hacia el objetivo
        float distancia = direccion.magnitude;

        float speed = agente.velocidad.magnitude;

        float t = distancia * speed;

        float prediccion;
        if (speed <= distancia / maxPrediction)
        {
            prediccion = maxPrediction;
        }
        else
        {
            prediccion = distancia * speed;
        }

        objetivo.transform.position = objetivoReal.transform.position;
        Vector3 posPredicha = objetivoReal.GetComponent<Rigidbody>().linearVelocity * prediccion;
        objetivo.transform.position += posPredicha;

        //Debug.Log("OBJETIVO REAL: " + objetivoReal.transform.position);
        //Debug.Log("PREDICCION: " + prediccion);

        if (debugPrediction)
        {
            Debug.DrawLine(agente.transform.position, objetivo.transform.position, new Color(1,0,0), 0.5f);
        }

        return base.GetComportamientoDireccion();
    }
}