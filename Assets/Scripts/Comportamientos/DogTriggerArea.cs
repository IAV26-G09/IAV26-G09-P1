using System;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEngine;

public class DogTriggerArea : MonoBehaviour
{
    [SerializeField] private float TriggerRadius = 1.5f;

    private SphereCollider triggerArea;
    private PerroState perroState;

    private Persecucion llegada;
    private Huir huir;

    List<Collider> collidedRats = new List<Collider>();

    void FixedUpdate()
    {
        collidedRats.Clear();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        perroState = GetComponentInParent<PerroState>();

        llegada = GetComponentInParent<Persecucion>();
        huir = GetComponentInParent<Huir>();

        huir.enabled = false;
        llegada.enabled = true;

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

    // Update is called once per frame
    void Update()
    {
        var rats = collidedRats.Count;
        Debug.Log(rats);

        if (rats >= 3)
        {
            perroState.SetState((int)PerroState.State.HUYENDO);

            huir.enabled = true;
            llegada.enabled = false;
        }
        else
        {
            perroState.SetState((int)PerroState.State.SIGUIENDO);

            huir.enabled = false;
            llegada.enabled = true;
        }
    }
}