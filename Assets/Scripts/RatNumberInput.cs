using System;
using TMPro;
using UCM.IAV.Movimiento;
using UnityEngine;
using UnityEngine.UI;

public class RatNumberInput : MonoBehaviour
{
    [SerializeField]
    private int _ratNumberInput;
    [SerializeField]
    private float _delay = 0.2f;

    private TMP_InputField _inputField;

    private void Start()
    {
        _inputField = this.GetComponent<TMP_InputField>();
    }

    // Recibe el numero modificado en el campo
    public void GetNumber()
    {
        _ratNumberInput = int.Parse(gameObject.GetComponent<TMP_InputField>().text);
    }

    // Gestiona el clic del boton 
    public void ButtonClick()
    {
        // calcula la diferencia entre las ratas que quieres y las que tienes
        int diff = Math.Abs(_ratNumberInput - GestorJuego.instance.getNumRats());

        //Debug.Log("Ratas act " + GestorJuego.instance.getNumRats());
        //Debug.Log("Ratas input " + _ratNumberInput);
        //Debug.Log("Diff " + diff);

        // quieres mas ratas de las que tienes -> te faltan ratas
        if (_ratNumberInput > GestorJuego.instance.getNumRats())
        {
            //for (int i = 0; i < diff; i++)
            //{
            //    GestorJuego.instance.SpawnRata(); // spawnea las ratas que te faltan
            //}
            StartCoroutine(SpawnRatsWithDelay(diff));
        }
        // quieres menos ratas de las que tienes -> te sobran ratas
        else if (_ratNumberInput < GestorJuego.instance.getNumRats())
        {
            //for (int i = 0; i < diff; i++)
            //{
            //    GestorJuego.instance.DespawnRata(); // despawnea las ratas que te sobran
            //}
            StartCoroutine(DespawnRatsWithDelay(diff));
        }
    }

    private System.Collections.IEnumerator SpawnRatsWithDelay(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GestorJuego.instance.SpawnRata();
            yield return new WaitForSeconds(_delay);
        }
    }

    private System.Collections.IEnumerator DespawnRatsWithDelay(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GestorJuego.instance.DespawnRata();
            yield return new WaitForSeconds(_delay);
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && _inputField != null && _inputField.isFocused) 
        {
            ButtonClick(); 
        }
    }
}
