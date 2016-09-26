using GameController;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public InputController InputController { get; set; }
    public float MovingStatus { get; set; }

    void Update()
    {
        if(MovingStatus != 0)
            InputController.InvokeMoveEvent(MovingStatus);

        InputController.ExecutingCommands();
    }

    private void _Move(float axis)
    {
        MovingStatus = axis;
    }

    public void Cry()
    {
        InputController.InvokeCryEvent(1);
    }

    public void Block()
    {
        InputController.InvokeBlockEvent(1);
    }
}
