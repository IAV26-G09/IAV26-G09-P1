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
    private GameObject objetivoReal; // Aqui le pasas el flautista,
                                     // en el objetivo del agente se le pasa un gameobject vacio
                                     // con posicion a predecir en cada update

    [SerializeField] private bool debugPrediction = false; // Para mostrar una linea desde el agente hasta el objetivo predicho

    private void Start()
    {
        if (objetivoReal== null)
        {
            objetivoReal = new GameObject();
        }
    }

    public override ComportamientoDireccion GetComportamientoDireccion()
    {
        // direccion hacia el objetivo predicho
        Vector3 direccion = objetivo.transform.position - agente.transform.position;
        // distancia hacia el objetivo predicho
        float distancia = direccion.magnitude;

        float speed = agente.getVelocidadLinealReal().magnitude;

        float prediccion;
        if (speed <= distancia / maxPrediction)
        {
            prediccion = maxPrediction;
        }
        else
        {
            // tiempo predicho que se tardaria en recorrer esa distancia
            prediccion = distancia / speed;
        }

        // copiamos los datos del objetivo real y lo traducimos a su prediccion
        objetivo.transform.position = objetivoReal.transform.position;
        Vector3 posPredicha = objetivoReal.GetComponent<Agente>().getVelocidadLinealReal() * prediccion;
        objetivo.transform.position += posPredicha;

        // Debug
        if (debugPrediction)
        {
            Debug.DrawLine(agente.transform.position, objetivo.transform.position, new Color(1,0,0), 0.5f);
        }

        // se realiza la llegada
        return base.GetComportamientoDireccion();
    }
}