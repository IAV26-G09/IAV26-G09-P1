/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// La clase Agente es responsable de modelar los agentes y gestionar todos los comportamientos asociados para combinarlos (si es posible).
    /// </summary>
    public class Agente : MonoBehaviour 
    {
        /// <summary>
        /// Combinar por peso
        /// </summary>
        [Tooltip("Combinar por peso")]
        public bool combinarPorPeso = false;

        /// <summary>
        /// Combinar por prioridad
        /// </summary>
        [Tooltip("Combinar por prioridad")]
        public bool combinarPorPrioridad = false;

        /// <summary>
        /// Umbral de prioridad para tener el valor en cuenta
        /// </summary>
        [Tooltip("Umbral de prioridad")]
        public float umbralPrioridad = 0.2f;

        /// <summary>
        /// Velocidad maxima
        /// </summary>
        [Tooltip("Velocidad (lineal) maxima")]
        public float velocidadMax;

        /// <summary>
        /// Rotacion maxima
        /// </summary>
        [Tooltip("Rotacion (velocidad angular) maxima")]
        public float rotacionMax;

        /// <summary>
        /// Aceleracion maxima
        /// </summary>
        [Tooltip("Aceleracion (lineal) maxima")]
        public float aceleracionMax;

        /// <summary>
        /// Aceleracion angular maxima
        /// </summary>
        [Tooltip("Aceleracion angular maxima")]
        public float aceleracionAngularMax;

        /// <summary>
        /// Velocidad (se puede dar una velocidad de inicio).
        /// </summary>
        [Tooltip("Velocidad")]
        public Vector3 velocidad;

        /// <summary>
        /// Rotacion (o velocidad angular; se puede dar una rotacion de inicio)
        /// </summary>
        [Tooltip("Rotacion (velocidad angular)")]
        public float rotacion;

        /// <summary>
        /// Orientacion (hacia donde encara el agente)
        /// </summary>
        [Tooltip("Orientacion")]
        public float orientacion;

        /// <summary>
        /// Valor del comportamiento de direccion (instrucciones de movimiento)
        /// </summary>
        [Tooltip("Comportamiento de direccion (instrucciones de movimiento)")]
        protected ComportamientoDireccion direccion;

        /// <summary>
        /// Grupos de direcciones, organizados segun su prioridad
        /// </summary>
        [Tooltip("Grupos de direcciones")]
        private Dictionary<int, List<ComportamientoDireccion>> grupos;

        /// <summary>
        /// Componente de cuerpo rigido (si la tiene el agente)
        /// </summary>
        [Tooltip("Cuerpo rigido")]
        private Rigidbody cuerpoRigido;

        /// <summary>
        /// Constante del tiempo de giro
        /// </summary>
        [Tooltip("Tiempo de giro (al cambiar de direccion)")]
        private float tiempoGiroSuave = 0.1f;

        /// <summary>
        /// Variable de referencia para damping
        /// </summary>
        [Tooltip("Referencia para el giro")]
        float velocidadGiroSuave;

        /// <summary>
        /// Al comienzo, se inicialian algunas variables
        /// </summary>
        void Start()
        {
            // Descomentar estas lineas si queremos ignorar los valores iniciales de velocidad y rotacion
            //velocidad = Vector3.zero; 
            //rotacion = 0.0f
            direccion = new ComportamientoDireccion();
            grupos = new Dictionary<int, List<ComportamientoDireccion>>();
            orientacion = transform.eulerAngles.y; // La orientacion inicial es la real

            cuerpoRigido = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// En cada tick fijo, si hay movimiento din�mico, uso el simulador fisico aplicando las fuerzas que corresponda para moverlo.
        /// Un cuerpo rigido se puede mover con movePosition, cambiando la velocity o aplicando fuerzas, que es lo habitual y que permite respetar otras fuerzas que est�n actuando sobre �l a la vez.
        /// </summary>
        public virtual void FixedUpdate()
        {
            if (cuerpoRigido.isKinematic)
                return; // El movimiento sera cinematico, fotograma a fotograma con Update

            // Limitamos la aceleracion al maximo que acepta este agente (aunque normalmente vendra ya limitada)
            if (direccion.lineal.sqrMagnitude > aceleracionMax)
                direccion.lineal = direccion.lineal.normalized * aceleracionMax; 

            // La opcion por defecto seria usar ForceMode.Force, pero eso implicaria que el comportamiento de direccion
            // tuviese en cuenta la masa a la hora de calcular la aceleracion que se pide
            cuerpoRigido.AddForce(direccion.lineal, ForceMode.Acceleration);

            // Limitamos la aceleracion angular al maximo que acepta este agente (aunque normalmente vendra ya limitada)
            if (direccion.angular > aceleracionAngularMax)
                direccion.angular = aceleracionAngularMax;

            // Rotamos el objeto siempre sobre su eje Y (hacia arriba), asumiendo que el agente est� sobre un plano y quiere mirar a un lado o a otro
            // La opcion por defecto seria usar ForceMode.Force, pero eso implicaria que el comportamiento de direccion tuviese en cuenta la masa a la hora de calcular la aceleraci�n que se pide
            cuerpoRigido.AddTorque(transform.up * direccion.angular, ForceMode.Acceleration);

            LookDirection();

            // Aunque tambien se controlen los maximos en el LateUpdate, entiendo que conviene tambien hacerlo aqui, en FixedUpdate,
            // que puede llegar a ejecutarse mas veces

            // Limito la velocidad lineal al terminar 
            if (cuerpoRigido.linearVelocity.magnitude > velocidadMax)
                cuerpoRigido.linearVelocity = cuerpoRigido.linearVelocity.normalized * velocidadMax;

            // Limito la velocidad angular al terminar
            if (cuerpoRigido.angularVelocity.magnitude > rotacionMax)
                cuerpoRigido.angularVelocity = cuerpoRigido.angularVelocity.normalized * rotacionMax;
            if (cuerpoRigido.angularVelocity.magnitude < -rotacionMax)
                cuerpoRigido.angularVelocity = cuerpoRigido.angularVelocity.normalized * -rotacionMax;
        }

        /// <summary>
        /// En cada tick, hace lo basico del movimiento cinem�tico del agente
        /// Un objeto que no atiende a fisicas se mueve a base de trasladar su transformada.
        /// Al no haber Freeze Rotation, ni rozamiento ni nada... seguramente vaya todo mucho mas rapido en cinematico que en dinamico
        /// </summary>
        public virtual void Update()
        {
            if (!cuerpoRigido.isKinematic)
                return; // El movimiento sera dinamico, controlado por la fisica y FixedUpdate

            // Limito la velocidad lineal antes de empezar
            if (velocidad.magnitude > velocidadMax)
                velocidad= velocidad.normalized * velocidadMax;

            // Limito la velocidad angular antes de empezar
            if (rotacion > rotacionMax)
                rotacion = rotacionMax;
            if (rotacion < -rotacionMax)
                rotacion = -rotacionMax;

            Vector3 desplazamiento = velocidad * Time.deltaTime;
            transform.Translate(desplazamiento, Space.World);

            orientacion += rotacion * Time.deltaTime;
            // Vamos a mantener la orientacion siempre en el rango can�nico de 0 a 360 grados
            if (orientacion < 0.0f)
                orientacion += 360.0f;
            else if (orientacion > 360.0f)
                orientacion -= 360.0f;

            LookDirection();

            // Elimino la rotacion totalmente, dej�ndolo en el estado inicial,
            // antes de rotar el objeto lo que nos marque la variable orientacion
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, orientacion);

        }

        /// <summary>
        /// En cada parte tardia del tick, hace tareas de correccion numerica 
        /// </summary>
        public virtual void LateUpdate()
        {
            if (combinarPorPrioridad)
            {
                direccion = GetPrioridadComportamientoDireccion();
                grupos.Clear();
            }

            if (cuerpoRigido != null) {
                return; // El movimiento sera dinamico, controlado por la fisica y FixedUpdate
            }

            // Limitamos la aceleracion al maximo que acepta este agente (aunque normalmente vendra ya limitada)
            if (direccion.lineal.sqrMagnitude > aceleracionMax)
                direccion.lineal = direccion.lineal.normalized * aceleracionMax;

            // Limitamos la aceleracion angular al m�ximo que acepta este agente (aunque normalmente vendr� ya limitada)
            if (direccion.angular > aceleracionAngularMax)
                direccion.angular = aceleracionAngularMax;

            // Aqu� se calcula la proxima velocidad y rotaci�n en funci�n de las aceleraciones  
            velocidad += direccion.lineal * Time.deltaTime;
            rotacion += direccion.angular * Time.deltaTime;

            // Opcional: Esto es para actuar con contundencia si nos mandan parar (no es muy realista)
            if (direccion.angular == 0.0f) 
                rotacion = 0.0f; 
            if (direccion.lineal.sqrMagnitude == 0.0f) 
                velocidad = Vector3.zero; 

            // En cada parte tardia del tick, encarar el agente (al menos para el avatar)...
            // si es que queremos hacer este encaramiento
            transform.LookAt(transform.position + velocidad);

            // Se deja la direccion vacia para el proximo fotograma
            direccion = new ComportamientoDireccion();
        }


        /// <summary>
        /// Establece la direccion tal cual
        /// </summary>
        public void SetComportamientoDireccion(ComportamientoDireccion direccion)
        {
            this.direccion = direccion;
        }

        /// <summary>
        /// Establece la direccion por peso
        /// </summary>
        /// <param name="direccion"></param>
        /// <param name="peso"></param>
        public void SetComportamientoDireccion(ComportamientoDireccion direccion, float peso)
        {
            this.direccion.lineal += (peso * direccion.lineal);
            this.direccion.angular += (peso * direccion.angular);

            Vector3 aceleracionMaxVec = aceleracionMax * direccion.lineal.normalized;
            direccion.lineal = Vector3.Max(direccion.lineal, aceleracionMaxVec);
            direccion.angular = Mathf.Max(direccion.angular, aceleracionAngularMax);
        }

        /// <summary>
        /// Establece la direccion por prioridad
        /// </summary>
        /// <param name="direccion"></param>
        /// <param name="prioridad"></param>
        public void SetComportamientoDireccion(ComportamientoDireccion direccion, int prioridad)
        {
            if (!grupos.ContainsKey(prioridad))
            {
                grupos.Add(prioridad, new List<ComportamientoDireccion>());
            }
            grupos[prioridad].Add(direccion);
        }

        /// <summary>
        /// Devuelve el valor de direccion calculado por prioridad
        /// </summary>
        /// <returns></returns>
        private ComportamientoDireccion GetPrioridadComportamientoDireccion()
        {
            ComportamientoDireccion direccion = new ComportamientoDireccion();
            List<int> gIdList = new List<int>(grupos.Keys);
            gIdList.Sort();
            foreach (int gid in gIdList)
            {
                ComportamientoDireccion direccionGrupo = new ComportamientoDireccion();
                foreach (ComportamientoDireccion direccionIndividual in grupos[gid])
                {
                    // Dentro del grupo la mezcla deber�a ser por peso
                    direccionGrupo.lineal += direccionIndividual.lineal;
                    direccionGrupo.angular += direccionIndividual.angular;
                }
                // Acumular la direccion del grupo
                direccion.lineal += direccionGrupo.lineal;
                direccion.angular += direccionGrupo.angular;

                // Solo si el resultado supera un umbral, entonces nos quedamos con esta salida y dejamos de mirar grupos con menos prioridad
                if (direccion.lineal.magnitude > umbralPrioridad
                     || Mathf.Abs(direccion.angular) > umbralPrioridad)
                {
                    return direccion;
                }
            }
            return direccion;
        }

        /// <summary>
        /// Calculates el Vector3 dado un cierto valor de orientacion
        /// </summary>
        /// <param name="orientacion"></param>
        /// <returns></returns>
        public Vector3 OriToVec(float orientacion)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(orientacion * Mathf.Deg2Rad) * 1.0f; //  * 1.0f se anade para asegurar que el tipo es float
            vector.z = Mathf.Cos(orientacion * Mathf.Deg2Rad) * 1.0f; //  * 1.0f se anade para asegurar que el tipo es float
            return vector.normalized;
        }

        private void LookDirection()
        {
            if (direccion.lineal.x != 0 || direccion.lineal.z != 0)
            {
                //Rotacion del personaje hacia donde camina (suavizado)
                float anguloDestino = Mathf.Atan2(direccion.lineal.x, direccion.lineal.z) * Mathf.Rad2Deg;
                //Esto es raro pero Brackeys dice que funciona
                float anguloSuave = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloDestino, ref velocidadGiroSuave, tiempoGiroSuave);

                transform.rotation = Quaternion.Euler(0f, anguloSuave, 0f);
            }
        }

        // dado que la velocidad se aplica el el LateUpdate no podemos acceder a la velocidad y
        // siempre es 0 cuando implementamos métodos de comportamientos,
        // esto nos devuelve la velocidad real que tiene el rigidbody en el momento dado
        public Vector3 getVelocidadLinealReal() 
        {
            return (cuerpoRigido != null && !cuerpoRigido.isKinematic) ? cuerpoRigido.linearVelocity : velocidad;
        }
    }
}
