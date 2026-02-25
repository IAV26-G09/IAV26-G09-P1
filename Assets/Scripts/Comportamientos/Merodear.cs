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
        [SerializeField]
        private float wanderOffset = 3.0f; // forward offset of wander circle
        [SerializeField]
        private float wanderRadius = 5.0f; // radius of wander circle

        private float wanderOrientation = 0.0f;

        [SerializeField] private bool debugMerodeo = false;

        [SerializeField]
        float tiempoMaximo = 2.0f; // maximum rate of wanderers orientation change
        [SerializeField]
        float tiempoMinimo = 1.0f; // maximum rate of wanderers orientation change

        //float currentTimeObjective = 0.0f;
        //float limitTimeObjective = 0.0f;

        //float currentTimeIdle = 0.0f;
        //float limitTimeIdle = 0.0f;

        private bool idle = false;
        private float timer = 0.0f;
        private float duration = 0.0f;

        private void Start()
        {
            if (objetivo == null)
            {
                objetivo = new GameObject();
            }
            duration = Random.Range(tiempoMinimo, tiempoMaximo);
        }

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            timer += Time.deltaTime;

            if (idle)
            { // si estamos en estado de idle
                if (timer >= duration) // si se ha de acabar el idle
                {
                    // --- 1: NUEVO OBJETIVO
                    // actualiza la direccion de merodeo
                    wanderOrientation = Random.Range(.0f, 360f);

                    // calcula el centro del wander circle
                    objetivo.transform.position = agente.transform.position + wanderOffset * agente.OriToVec(wanderOrientation);

                    // calcula targetOrientation
                    float targetOrientation = wanderOrientation + agente.orientacion;

                    // posicion del target
                    objetivo.transform.position += wanderRadius * agente.OriToVec(targetOrientation);

                    // salir del idle
                    idle = false;
                    timer = 0f;
                }
                // si estamos en idle no hacemos nada
                return result;
            }

            // --- 2: CÁLCULO HASTA EL OBJETIVO
            // direccion hacia el objetivo
            Vector3 direccion = objetivo.transform.position - agente.transform.position;

            // distancia hacia el objetivo
            float distancia = direccion.magnitude;

            if (timer >= duration || distancia < wanderRadius) // si ha pasado suficiente tiempo o ha llegado al radio objetivo
            {
                idle = true;
                timer = 0f;
                duration = Random.Range(tiempoMinimo, tiempoMaximo);

                return result;
            }

            direccion.Normalize();

            result.angular = 0;

            // --- 3
            // lineal = max aceleracion en la direccion de la orientacion
            result.lineal = agente.aceleracionMax * direccion;

            if (debugMerodeo)
            {
                Debug.DrawLine(agente.transform.position, objetivo.transform.position, new Color(0, 0, 1), 0.2f);
            }

            return result;
        }
    }
}