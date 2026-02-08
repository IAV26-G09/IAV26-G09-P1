using UCM.IAV.Movimiento;
using UnityEngine;

public class ReiniciarButton : MonoBehaviour
{
    public void RestartButton()
    {
        GestorJuego.instance.Restart();
    }

}
