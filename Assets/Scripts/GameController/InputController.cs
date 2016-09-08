using UnityEngine;
using GameController.Commands;

namespace GameController
{

    public class InputController : MonoBehaviour
    {
        public static InputController Instance { get; private set; }

        public delegate void ExecuteMethod();
        public delegate ICommand Command(float axis);

        public event Command MoveEvent;
        public event Command CryEvent;

        // Use this for initialization
        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Moving") != 0 && MoveEvent != null)
                _executeCommand(MoveEvent.Invoke(Input.GetAxis("Moving")));

            if (Input.GetAxis("Cry") != 0 && CryEvent != null)
                _executeCommand(CryEvent.Invoke(Input.GetAxis("Cry")));
        }

        private void _executeCommand(ICommand command)
        {
            if (command != null) command.Execute();
        }
    }
}