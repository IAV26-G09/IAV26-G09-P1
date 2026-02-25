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
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        // Entidades potenciales de las que huir
        public GameObject targEmpty; // se inicializa con el gameobject contenedor que contiene todas las ratas

        // Umbral en el que se activa
        [SerializeField]
        float umbral;

        // Coeficiente de reducción de la fuerza de repulsión
        // Indica lo rápido que decae la separación con la distancia
        [SerializeField]
        float decayCoefficient;

        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>
        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            foreach(Transform target in targEmpty.transform)
            {
                if (target == this.transform) // no cuenta al GameObject contenedor
                    continue;

                // Direccion en el sentido contrario al target
                Vector3 direction = agente.transform.position - target.position;
                float distance = direction.magnitude;

                if (distance < 0.0001f)
                    continue;

                // si el target esta suficientemente cerca
                if (distance < umbral)
                {
                    // Calcula la fuerza de la repulsion -> INVERSE SQUARE LAW
                    float strength = Mathf.Min(
                        decayCoefficient / (Mathf.Pow(distance, 2)), 
                        agente.aceleracionMax);

                    direction.Normalize();
                    result.lineal += strength * direction;
                }
            }

            return result;
        }
    }
}