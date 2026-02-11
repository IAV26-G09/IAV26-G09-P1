using UnityEngine;

public class PerroState : MonoBehaviour
{
    public enum State : int
    {
        SIGUIENDO = 0,
        HUYENDO = 1
    }

    [SerializeField]
    private int currentState = 0;

    public int GetState()
    {
        return currentState;
    }

    public void SetState(int s)
    {
        currentState = s;

        if (currentState == (int)State.SIGUIENDO)
        {
            Debug.Log("Current state: siguiendo");
        }
        else if(currentState == (int)State.HUYENDO)
        {
            Debug.Log("Current state: huyendo");
        }
    }
}
