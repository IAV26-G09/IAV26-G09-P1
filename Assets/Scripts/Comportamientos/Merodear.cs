/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        // ----
        private float wanderOffset = 3.0f; // forward offset of wander circle
        private float wanderRadius = 5.0f; // radius of wander circle

        [SerializeField]
        float tiempoMaximo = 2; // maximum rate of wanderers orientation change

        [SerializeField]
        float tiempoMinimo = 1; // maximum rate of wanderers orientation change

        float current = 0.0f;
        float limit = 0.0f;

        private float wanderOrientation = 0.0f; 

        private void Start()
        {
            if (objetivo == null)
            {
                objetivo = new GameObject();
            }
        }

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            current += Time.deltaTime;

            if (current >= limit)
            {
                current = 0;
                limit = Random.Range(tiempoMinimo, tiempoMaximo);

                // --- 1
                // actualiza la direccion de merodeo
                wanderOrientation = Random.Range(.0f, 360f);

                // calcula el centro del wander circle
                objetivo.transform.position = agente.transform.position + wanderOffset * agente.OriToVec(wanderOrientation);

                // calcula targetOrientation
                float targetOrientation = wanderOrientation + agente.orientacion;

                // posicion del target
                objetivo.transform.position += wanderRadius * agente.OriToVec(targetOrientation);

                // --- 2
                // direccion hacia el objetivo
                Vector3 direccion = objetivo.transform.position - agente.transform.position;

                // distancia hacia el objetivo
                float distancia = direccion.magnitude;

                // hemos llegado -> paramos
                if (distancia < wanderRadius)
                {
                    return result; // intencion de detenerse
                }

                result.angular = 0;

                // --- 3
                // lineal = max aceleracion en la direccion de la orientacion
                result.lineal = agente.aceleracionMax * direccion;
            }

            return result;
        }

    }
}
