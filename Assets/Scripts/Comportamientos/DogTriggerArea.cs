using System;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DogTriggerArea : MonoBehaviour
{
    [SerializeField] private float TriggerRadius = 1.5f; // radio de trigger del perro
    [SerializeField] private int ratsToFlee = 3; // numero de ratas en trigger para huir

    private SphereCollider triggerArea;
    private Persecucion persecucion;
    private Huir huir;

    List<Collider> collidedRats = new List<Collider>(); // para ir comprobando cuantas ratas tiene el agente cerca

    private GameObject centroidObject; // objeto vacio para calcular el centroide, sera el objetivo del que huir
    [SerializeField] private bool debugCentroide = true; // para dibujar l�nea entre el centroide calculado 

    void FixedUpdate()
    {
        collidedRats.Clear();
    }

    void Start()
    {
        persecucion = GetComponentInParent<Persecucion>();
        huir = GetComponentInParent<Huir>();

        centroidObject = new GameObject();

        triggerArea = GetComponent<SphereCollider>();
        if (triggerArea != null)
            triggerArea.radius = TriggerRadius; // valor dado a la que las ratas triggerean la huida del perro
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    void OnTriggerEnter(Collider other)
    {
        // si ese collider no ha sido registrado
        if (!collidedRats.Contains(other) &&
            other.gameObject.tag.Equals("Rat")) // solo registrar las ratas
        {
            collidedRats.Add(other); // la mete en la lista
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rat"))
            collidedRats.Remove(other); // la saca de la lista
    }

    void Update()
    {
        var rats = collidedRats.Count; // las ratas que haya en ese momento en la lista

        // Establecer el estado del perro en funcion de las ratas que 
        // tenga a TriggerRadius de distancia
        if (rats >= ratsToFlee)
        {
            huir.isFleeing = true; // activa el comportamiento de huida

            Vector3 suma = Vector3.zero;
            Vector3 dogPos = transform.position;
            float pesoTotal = 0f;

            foreach (Collider c in collidedRats)
            {
                Vector3 ratPos = c.transform.position; // posicion de la rata que estamos calculando
                float distance = Vector3.Distance(dogPos, ratPos); // distancia a esa rata

                distance = Mathf.Clamp(distance, 0.01f, distance); // para que la distancia nunca sea 0

                float peso = ((1f / distance)); // calcula su peso en funcion de la distancia 
                // de esta forma el centroide se acercara mas a las ratas con mayor peso simulando 
                // una mayor alteracion del perro a menor distancia

                pesoTotal += peso;
                suma += ratPos * peso; // va sumando todas las posiciones
            }

            Vector3 centroide = suma / pesoTotal; // se calcula el centroide ajustandolo para que los pesos esten en el rango [0-1]
            centroidObject.transform.position = centroide; // se cambia la posicion del gameobject centroide
            huir.objetivo = centroidObject; // se establece ese centroide como objetivo de huida

            // Debug
            if (debugCentroide)
            {
                Debug.DrawLine(transform.position, centroidObject.transform.position, new Color(1, 1, 0), 0.5f);
            }
        }
        else
        {
            huir.isFleeing = false; // desactiva el comportamiento de huida
            huir.objetivo = persecucion.objetivo;
        }
    }
}