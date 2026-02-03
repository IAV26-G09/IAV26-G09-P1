using System;
using TMPro;
using UCM.IAV.Movimiento;
using UnityEngine;
using UnityEngine.UI;

public class RatNumberInput : MonoBehaviour
{
    [SerializeField]
    private int _ratNumberInput;

    // Recibe el numero modificado en el campo
    public void GetNumber()
    {
        _ratNumberInput = int.Parse(gameObject.GetComponent<TMP_InputField>().text);
        Debug.Log("INPUTTTTTT " + _ratNumberInput);
    }

    // Gestiona el clic del boton 
    public void ButtonClick()
    {
        Debug.Log("HOLA");

        // calcula la diferencia entre las ratas que quieres y las que tienes
        int diff = Math.Abs(_ratNumberInput - GestorJuego.instance.getNumRats());

        Debug.Log("Ratas act " + GestorJuego.instance.getNumRats());
        Debug.Log("Ratas input " + _ratNumberInput);
        Debug.Log("Diff " + diff);

        // quieres mas ratas de las que tienes -> te faltan ratas
        if (_ratNumberInput > GestorJuego.instance.getNumRats())
        {
            for (int i = 0; i < diff; i++)
            {
                GestorJuego.instance.SpawnRata(); // spawnea las ratas que te faltan
            }
        }
        // quieres menos ratas de las que tienes -> te sobran ratas
        else if (_ratNumberInput < GestorJuego.instance.getNumRats())
        {
            for (int i = 0; i < diff; i++)
            {
                GestorJuego.instance.DespawnRata(); // despawnea las ratas que te sobran
            }
        }
    }
}
