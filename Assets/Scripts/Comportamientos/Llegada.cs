/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>


        // El radio para llegar al objetivo
        public float rObjetivo;

        // El radio en el que se empieza a ralentizarse
        public float rRalentizado;

        // El tiempo en el que conseguir la aceleracion objetivo
        float timeToTarget = 0.1f;

        public float distancia = 7.0f;

        public float fRalentizado;

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            // IMPLEMENTAR llegada

            ComportamientoDireccion result =  new ComportamientoDireccion();

            // direccion hacia el objetivo
            Vector3 direccion = objetivo.transform.position - agente.transform.position;
            // distancia hacia el objetivo
            distancia = direccion.magnitude;

            // hemos llegado -> paramos
            if (distancia < rObjetivo)
            {
                return result; // intencion de detenerse
            }

            // no hemos llegado y fuera del radio de realentizado -> avanzamos a maxima velocidad
            float targetSpeed = 0;
            if (distancia > rRalentizado)
            {
                targetSpeed = agente.velocidadMax;
            }
            // calcular velocidad escalada 
            else
            {
                targetSpeed = agente.velocidadMax * distancia / rRalentizado;
            }

            // la velocidad deseada combina velocidad y direccion
            Vector3 targetVelocity = direccion;
            targetVelocity.Normalize();
            targetVelocity *= targetSpeed;

            // la aceleracion escala hasta la velocidad deseada
            result.lineal = targetVelocity - agente.velocidad;
            result.lineal /= timeToTarget;

            // comprueba si la aceleracion es demasiado rapida
            if (result.lineal.magnitude > agente.aceleracionMax)
            {
                result.lineal.Normalize();
                result.lineal *= agente.aceleracionMax;
            }

            result.angular = 0;

            return result;
        }
    }
}
