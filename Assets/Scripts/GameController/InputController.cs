using UnityEngine;

namespace GameController
{

    public class InputController : MonoBehaviour
    {
        public static InputController Instance { get; private set; }

        public delegate void ExecuteMethod();
        public delegate void Command(float axis);

        public event Command MoveEvent;
        public event Command CryEvent;
        public event Command BlockEvent;

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
                MoveEvent.Invoke(Input.GetAxis("Moving"));

            if (Input.GetAxis("Cry") != 0 && CryEvent != null)
                CryEvent.Invoke(Input.GetAxis("Cry"));

            if (Input.GetAxis("Block") != 0 && BlockEvent != null)
                BlockEvent.Invoke(Input.GetAxis("Block"));

            _ExecutingCommands();
        }

        private void _ExecutingCommands()
        {
            while (CommandManager.CountCommands > 0)
            {
                CommandManager.ExecuteNextCommand();
            }
        }
    }
}