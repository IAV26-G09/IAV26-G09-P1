/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System;

namespace UCM.IAV.Movimiento
{

    using UnityEngine;
    using static UnityEditor.FilePathAttribute;

    /// <summary>
    /// Clase abstracta que funciona como plantilla para todos los comportamientos de agente
    /// </summary>
    public class ComportamientoAgente : MonoBehaviour
    {
        /// <summary>
        /// Peso
        /// </summary>
        public float peso = 1.0f;
        /// <summary>
        /// Prioridad
        /// </summary>
        public int prioridad = 1;
        /// <summary>
        /// Objetivo (para aplicar o representar el comportamiento, depende del comportamiento que sea)
        /// </summary>
        public GameObject objetivo;
        /// <summary>
        /// Agente que hace uso del comportamiento
        /// </summary>
        protected Agente agente;

        /// <summary>
        /// Al despertar, establecer el agente que hará uso del comportamiento
        /// </summary>
        public virtual void Awake()
        {
            agente = gameObject.GetComponent<Agente>();
        }

        /// <summary>
        /// En cada tick, establecer la dirección que corresponde al agente, con peso o prioridad si se están usando
        /// </summary>
        public virtual void Update()
        {
            if (agente == null)
            {
                Debug.LogWarning("Agente no encontrado en " + gameObject.name);
                return;
            }

            if (agente.combinarPorPeso)
                agente.SetComportamientoDireccion(GetComportamientoDireccion(), peso);
            else if (agente.combinarPorPrioridad)
                agente.SetComportamientoDireccion(GetComportamientoDireccion(), prioridad);
            else
                agente.SetComportamientoDireccion(GetComportamientoDireccion());
        }

        /// <summary>
        /// Devuelve el comportamiento de dirección calculado
        /// </summary>
        /// <returns></returns>
        public virtual ComportamientoDireccion GetComportamientoDireccion()
        {
            return new ComportamientoDireccion();
        }

        /// <summary>
        /// Asocia la rotación al rango de 360 grados
        /// </summary>
        /// <param name="rotacion"></param>
        /// <returns></returns>
        public float MapToRange(float rotacion)
        {
            rotacion %= 360.0f;
            if (Mathf.Abs(rotacion) > 180.0f)
            {
                if (rotacion < 0.0f)
                    rotacion += 360.0f;
                else
                    rotacion -= 360.0f;
            }
            return rotacion;
        }

        /// <summary>
        /// Cambia el valor real de la orientación a un Vector3 
        /// </summary>
        /// <param name="orientacion"></param>
        /// <returns></returns>
        public Vector3 OriToVec(float orientacion)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(orientacion * Mathf.Deg2Rad) * 1.0f;
            vector.z = Mathf.Cos(orientacion * Mathf.Deg2Rad) * 1.0f;
            return vector.normalized;
        }

        
        public ComportamientoDireccion GetSteeringSeek()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            // direccion hacia el objetivo
            result.lineal = objetivo.transform.position - agente.transform.position;

            // maxima aceleracion en esa direccion
            result.lineal.Normalize();
            result.lineal *= agente.aceleracionMax;

            result.angular = 0;

            return result;
        }

        public ComportamientoDireccion GetSteeringAlign()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            // El radio para llegar al objetivo
            float rObjetivo = 1.0f;

            // El radio en el que se empieza a ralentizarse
            float rRalentizado = 30.0f;

            // El tiempo en el que conseguir la aceleracion objetivo
            float timeToTarget = 0.1f;

            float rotacion = objetivo.transform.eulerAngles.y - agente.orientacion;

            // mapear el resultado al rango de -180 a 180
            //agente.rotacion = (float)((Math.PI / 180) * objetivo.transform.rotation.eulerAngles.y);
            rotacion = MapToRange(rotacion);
            float rotationSize = Mathf.Abs(rotacion);

            // comprobar si estamos ahi, devolver ninguna rotacion
            if (rotationSize < rObjetivo)
            {
                return result;
            }

            // si estamos fuera del radio de ralentizado, entonces usamos la maxima rotacion
            float targetRotation = 0;
            if (rotationSize > rRalentizado)
            {
                targetRotation = agente.rotacionMax;
                //objetivo.
            }
            else // si no calculamos la escalada
            {
                targetRotation = agente.rotacionMax * rotationSize / rRalentizado;
            }

            // la rotacion deseada final combina velocidad y direccion
            targetRotation *= rotacion / rotationSize;

            // la aceleracion intenta llegar a la rotacion deseada
            result.angular = targetRotation - agente.rotacion;
            result.angular /= timeToTarget;

            // comprobar si la aceleracion es demasiada
            float angularAcceleration = Mathf.Abs(result.angular);
            if (angularAcceleration > agente.aceleracionAngularMax)
            {
                result.angular /= angularAcceleration;
                result.angular *= agente.aceleracionAngularMax;
            }

            result.lineal = Vector3.zero;
            return result;
        }

        public ComportamientoDireccion GetSteeringFace()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            // 1. calcular el objetivo al que delegar el anieamiento
            // deducir la direccion al target
            Vector3 direccion = objetivo.transform.position - agente.transform.position;

            // comprobar direccion 0
            if (direccion.magnitude == 0)
                return result;

            // 2. delegar a align
            return GetSteeringAlign();
        }
    }
}
