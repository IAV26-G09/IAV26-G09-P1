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

    List<Collider> collidedRats = new List<Collider>();
    private GameObject centroidObject;
    void FixedUpdate()
    {
        collidedRats.Clear();
    }

    void Start()
    {
        persecucion = GetComponentInParent<Persecucion>();
        huir = GetComponentInParent<Huir>();

        if (huir != null)
            huir.enabled = true;
        
        if (persecucion != null)
            persecucion.enabled = true;
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
            collidedRats.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rat"))
            collidedRats.Remove(other);
    }

    void Update()
    {
        var rats = collidedRats.Count;

        // Establecer el estado del perro en funcion de las ratas que 
        // tenga a TriggerRadius de distancia
        if (rats >= ratsToFlee)
        {
            huir.isFleeing = true;

            Vector3 suma = Vector3.zero;

            foreach (Collider c in collidedRats)
                suma += c.transform.position;

            Vector3 centroide = suma / rats;
            centroidObject.transform.position = centroide;
            huir.objetivo = centroidObject;
        }
        else
        {
            huir.isFleeing = false;
            huir.objetivo = persecucion.objetivo;
        }
    }
}