using System;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEngine;

public class DogTriggerArea : MonoBehaviour
{
    [SerializeField] private float TriggerRadius = 1.5f;

    private SphereCollider triggerArea;
    private PerroState perroState;

    List<Collider> collidedRats = new List<Collider>();

    void FixedUpdate()
    {
        collidedRats.Clear();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        perroState = GetComponentInParent<PerroState>();

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
            other.gameObject.tag.Equals("Player")) // solo registrar las ratas
        {
            collidedRats.Add(other);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var rats = collidedRats.Count;
        Debug.Log(rats);

        if (rats >= 1)
        {
            perroState.SetState((int)PerroState.State.HUYENDO);
        }
        else
        {
            perroState.SetState((int)PerroState.State.SIGUIENDO);
        }
    }
}