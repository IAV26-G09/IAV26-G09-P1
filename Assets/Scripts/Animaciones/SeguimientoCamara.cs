/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoCamara: MonoBehaviour
{
    public Transform flautistTarget;
    public Transform dogTarget;
    private Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public bool lookAt = true; // si esta fija o no
    [SerializeField]
    Vector3 fixedPosition; // posicion para la camara fija
    [SerializeField]
    Transform fixedCenter; // posicion centro

    private void Awake()
    {
        target = flautistTarget;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!lookAt) 
        {
            ColocarCamara(fixedPosition, fixedCenter);
        }
        else
        {
            ColocarCamara(target.position, target);
        }
    }

    public void ColocarCamara(Vector3 endPos, Transform target)
    {
        Vector3 pos = endPos + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, pos, smoothSpeed);

        transform.position = smoothPos;
        transform.LookAt(target);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
