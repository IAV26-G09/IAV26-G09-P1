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
    using UnityEngine;
    using UnityEngine.Playables;

    /// <summary>
    /// El comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador: ComportamientoAgente
    {

        [SerializeField]
        Transform transform;

        [SerializeField]
        float minimuRadius = 3.0f; // radio alrededor del jugador en el que no moverse

        private float velocidadMaxNormal;
        private float velocidadMaxRapida;
        private float aceleracionNormal;
        private float aceleracionRapida;

        private bool sprinting = false;
        private void Start()
        {
            transform = GetComponent<Transform>();
            velocidadMaxNormal = agente.velocidadMax;
            velocidadMaxRapida = velocidadMaxNormal * 2;

            aceleracionNormal = agente.aceleracionMax;
            aceleracionRapida = aceleracionNormal * 2;
        }

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion direccion = new ComportamientoDireccion();
            
            //Direccion actual
            // Control por teclado
            direccion.lineal.x = Input.GetAxis("Horizontal");
            direccion.lineal.z = Input.GetAxis("Vertical");

            // Control por raton
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Si apuntamos a un sitio valido
            if (Physics.Raycast(ray, out hit))
            { // Cogemos la direccion y nos congelamos en altura
                direccion.lineal = hit.point - transform.position;
                direccion.lineal.y = 0;
            }

            // Si la colisión, aunque válida está en un radio cercano al jugador
            if (direccion.lineal.magnitude < minimuRadius)
            {
                return new ComportamientoDireccion()    ;
            }

            // Comprobamos si estamos corriendo
            sprinting = Input.GetKey(KeyCode.Mouse0);

            if (sprinting)
            {
                agente.aceleracionMax = aceleracionRapida;
                agente.velocidadMax = velocidadMaxRapida;
            }
            else
            {
                agente.aceleracionMax = aceleracionNormal;
                agente.velocidadMax = velocidadMaxNormal;
            }

            //Resto de cálculo de movimiento
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            // Podríamos meter una rotación automática en la dirección del movimiento, si quisiéramos

            return direccion;
        }
    }
}