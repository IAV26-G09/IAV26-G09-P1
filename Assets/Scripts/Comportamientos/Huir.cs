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
    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class Huir : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            // direccion en sentido contrario al target
            result.lineal = agente.transform.position - objetivo.transform.position;

            result.lineal.Normalize();
            result.lineal *= agente.aceleracionMax;

            result.angular = 0;

            return result;
        }
    }
}
