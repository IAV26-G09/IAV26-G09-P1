/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>

        // Entidades potenciales de las que huir
        public GameObject targEmpty;

        // Umbral en el que se activa
        [SerializeField]
        float umbral;

        // Coeficiente de reducción de la fuerza de repulsión
        [SerializeField]
        float decayCoefficient;

        private GameObject[] targets;

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            foreach(var target in targets)
            {
                UnityEngine.Vector3 direction = target.transform.position - agente.transform.position;

                float distance = direction.magnitude;

                if (distance < umbral)
                {
                    // Calcula la fuerza de la repulsion -> INVERSE SQUARE LAW
                    float strength = Mathf.Min(decayCoefficient / (Mathf.Pow(distance, 2)), agente.aceleracionMax);

                    direction.Normalize();
                    result.lineal += strength * direction;
                }
            }

            return result;
        }
    }
}