using UnityEngine;
using GameController;

public class ClassicInput : MonoBehaviour
{
    public InputController InputController { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Moving") != 0)
            InputController.InvokeMoveEvent(Input.GetAxis("Moving"));

        if (Input.GetKeyDown(KeyCode.Space))
            InputController.InvokeCryEvent(Input.GetAxis("Cry"));

        if ((Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)))
            InputController.InvokeBlockEvent(Input.GetAxis("Block"));


        InputController.ExecutingCommands();
    }
}
