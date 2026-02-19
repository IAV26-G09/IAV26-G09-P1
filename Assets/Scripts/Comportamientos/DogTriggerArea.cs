using System;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DogTriggerArea : MonoBehaviour
{
    [SerializeField] private float TriggerRadius = 1.5f;
    [SerializeField] private int ratsToFlee = 3; // n ratas para huir

    private SphereCollider triggerArea;
    private Persecucion persecucion;
    private Huir huir;

    List<Collider> collidedRats = new List<Collider>();

    void FixedUpdate()
    {
        collidedRats.Clear();
    }

    void Start()
    {
        persecucion = GetComponentInParent<Persecucion>();
        huir = GetComponentInParent<Huir>();

        huir.enabled = true;
        persecucion.enabled = true;

        triggerArea = GetComponent<SphereCollider>();
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

        if (rats >= ratsToFlee)
        {
            huir.isFleeing = true;
        }
        else
        {
            huir.isFleeing = false;
        }
    }
}