/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Clase auxiliar para representar el comportamiento de dirección que debe usarse como guía para corregir dinámicamente (mediante aceleraciones) el movimiento del agente.
    /// </summary>
    public class ComportamientoDireccion
    {
        public float angular;
        public Vector3 lineal;
        public ComportamientoDireccion()
        {
            angular = 0.0f;
            lineal = new Vector3();
        }
    }
}