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
        float tiempoMaximoMovimiento = 2.0f; // tiempo maximo que una rata puede estar moviendoses
        [SerializeField]
        float tiempoMinimoMovimiento = 1.0f; // tiempo minimo que una rata puede estar moviendose
        [SerializeField]
        float tiempoMaximoIdle = 2.0f; // tiempo maximo que una rata puede estar descnasando
        [SerializeField]
        float tiempoMinimoIdle = 1.0f; // tiempo minimo que una rata puede estar descansando

        // estado ocioso
        private bool idle = true;
        [SerializeField]
        private float idleTimer = 0.0f; // contador
        [SerializeField]
        private float idleDuration = 0.0f; // tiempo determinado

        // estado movimiento
        [SerializeField]
        private float moveTimer = 0.0f; // contador
        [SerializeField]
        private float moveDuration = 0.0f; // tiempo determinado
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
                    // --- Calcula el nuevo objetivo
                    // actualiza la direccion de merodeo
                    wanderOrientation = Random.Range(.0f, 360f);

                    // calcula el centro del wander circle
                    objetivo.transform.position = agente.transform.position + wanderOffset * agente.OriToVec(wanderOrientation);

                    // calcula targetOrientation
                    float targetOrientation = wanderOrientation + agente.orientacion;

                    // posicion del target
                    objetivo.transform.position += wanderRadius * agente.OriToVec(targetOrientation);
                    objetivo.transform.position = new Vector3(objetivo.transform.position.x, 0, objetivo.transform.position.z);
                    // salir del idle
                    idle = false;
                    idleTimer = 0f;
                    moveTimer = 0f;
                    moveDuration = Random.Range(tiempoMinimoMovimiento, tiempoMaximoMovimiento);
                }
                // si estamos en idle no hacemos nada
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                return result;
            }

            moveTimer += Time.deltaTime;

            // Se dirige hacia el nuevo objetivo
            Vector3 direccion = objetivo.transform.position - agente.transform.position;
            float distancia = direccion.magnitude;

            // si ha pasado suficiente tiempo o ha llegado al radio objetivo
            if (moveTimer >= moveDuration || distancia < targetRadius) 
            {
                idle = true;

                moveTimer = 0f;
                idleTimer = 0f;
                idleDuration = Random.Range(tiempoMinimoIdle, tiempoMaximoIdle); // nueva duracion del idle

                return result;
            }

            direccion.Normalize();

            result.angular = 0;

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