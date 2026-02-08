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
using Random = UnityEngine.Random;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        float t = 3.0f;
        float actualT = 2.0f;

        // ----
        private float wanderOffset = 1.0f; // forward offset of wander circle
        private float wanderRadius = 5.0f; // radius of wander circle

        [SerializeField]
        float tiempoMaximo = 2.0f; // maximum rate of wanderers orientation change

        [SerializeField]
        float tiempoMinimo = 1.0f; // maximum rate of wanderers orientation change

        private float wanderOrientation = 0.0f; 

        ComportamientoDireccion lastDir = new ComportamientoDireccion(); // current orientation of wanderer

        //private Transform transform;
        private void Start()
        {
            if (objetivo == null)
            {
                objetivo = new GameObject();
            }
        }

        public override ComportamientoDireccion GetComportamientoDireccion()
        {

            // IMPLEMENTAR merodear
            tiempoMinimo = 1.0f; // por ejemplo
            tiempoMaximo = 2.0f; // por ejemplo
            actualT = 2.0f; // por ejemplo
            t = 3.0f; // por ejemplo

            /*
            wanderOrientation += Random.Range(0.0f, 360.0f) * tiempoMaximo;

            float targetOrientation = wanderOrientation;

            Vector3 target = transform.position + wanderOffset * agente.OriToVec(lastDir.angular);

            target += wanderRadius * agente.OriToVec(targetOrientation);

            /*
            result = 

            direccion.lineal = agente.aceleracionMax * (direccion.angular);*/

            // --- 1
            // actualiza la direccion de merodeo
            wanderOrientation += Random.Range(.0f, 360f) * tiempoMaximo;

            // calcula targetOrientation
            float targetOrientation = wanderOrientation + agente.orientacion;

            // calcula el centro del wander circle
            objetivo.transform.position = agente.transform.position + wanderOffset * agente.OriToVec(agente.orientacion);

            // posicion del target
            objetivo.transform.position += wanderRadius * agente.OriToVec(targetOrientation);

            // --- 2
            ComportamientoDireccion result = new ComportamientoDireccion();



            return result;
        }

    }
}
