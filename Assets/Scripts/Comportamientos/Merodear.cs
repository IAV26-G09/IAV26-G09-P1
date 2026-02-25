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
        float tiempoMaximoMovimiento = 2.0f; // maximum rate of wanderers orientation change
        [SerializeField]
        float tiempoMinimoMovimiento = 1.0f; // maximum rate of wanderers orientation change
        [SerializeField]
        float tiempoMaximoIdle = 2.0f; // maximum rate of wanderers orientation change
        [SerializeField]
        float tiempoMinimoIdle = 1.0f; // maximum rate of wanderers orientation change

        private bool idle = false;
        [SerializeField]
        private float idleTimer = 0.0f;
        [SerializeField]
        private float idleDuration = 0.0f;

        [SerializeField]
        private float moveTimer = 0.0f;
        [SerializeField]
        private float moveDuration = 0.0f;
        public float timeToTarget = 0.1f;

        [SerializeField] float targetRadius = 0.5f;   // cuando parar
        private Rigidbody rb;
        private void Start()
        {
            if (objetivo == null)
                objetivo = new GameObject();

            rb = GetComponent<Rigidbody>();

            idle = true;
            idleDuration = Random.Range(tiempoMinimoIdle, tiempoMaximoIdle);
        }

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            if (idle)
            { // si estamos en estado de idle
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleDuration)
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
                    idleTimer = 0f;
                    moveTimer = 0f;
                    moveDuration = Random.Range(tiempoMinimoMovimiento, tiempoMaximoMovimiento);
                }
                // si estamos en idle no hacemos nada
                return result;
            }

            moveTimer += Time.deltaTime;

            Vector3 direccion =
                objetivo.transform.position - agente.transform.position;

            // distancia hacia el objetivo
            float distancia = direccion.magnitude;

            if (moveTimer >= moveDuration || distancia < targetRadius) // si ha pasado suficiente tiempo o ha llegado al radio objetivo
            {
                idle = true;

                moveTimer = 0f;
                idleTimer = 0f;
                idleDuration = Random.Range(tiempoMinimoIdle, tiempoMaximoIdle);

                return result;
            }

            direccion.Normalize();

            result.angular = 0;

            // --- 3
            // lineal = max aceleracion en la direccion de la orientacion
            result.lineal = agente.aceleracionMax * direccion;

            // Debug
            if (debugMerodeo)
            {
                Debug.DrawLine(agente.transform.position, objetivo.transform.position, new Color(0, 0, 1), 0.2f);
            }

            return result;
        }
    }
}