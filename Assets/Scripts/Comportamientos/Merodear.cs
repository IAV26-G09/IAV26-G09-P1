/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        [SerializeField]
        float tiempoMaximo = 2.0f;

        [SerializeField]
        float tiempoMinimo = 1.0f;

        float t = 3.0f;
        float actualT = 2.0f;

        ComportamientoDireccion lastDir = new ComportamientoDireccion();

        public override ComportamientoDireccion GetComportamientoDireccion(){
            // IMPLEMENTAR merodear
            tiempoMinimo = 1.0f; // por ejemplo
            tiempoMaximo = 2.0f; // por ejemplo
            actualT = 2.0f; // por ejemplo
            t = 3.0f; // por ejemplo    
            return new ComportamientoDireccion();
        }

    }
}
