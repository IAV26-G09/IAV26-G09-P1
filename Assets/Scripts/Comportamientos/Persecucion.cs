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
    public GameObject objetivoReal; // objetivo real, el del agente será el predicho

    public override ComportamientoDireccion GetComportamientoDireccion()
    {
        // direccion hacia el objetivo
        Vector3 direccion = objetivo.transform.position - agente.transform.position;
        // distancia hacia el objetivo
        float distancia = direccion.magnitude;

        float speed = agente.velocidad.magnitude;

        float prediccion;
        if (speed <= distancia / maxPrediction)
        {
            prediccion = maxPrediction;
        }
        else
        {
            prediccion = distancia / speed;
        }

        GameObject objetivoPredicho = objetivoReal;
        objetivoPredicho.transform.position += objetivo.GetComponent<Agente>().velocidad * prediccion;

        objetivo = objetivoPredicho;

        return base.GetComportamientoDireccion();
    }
}