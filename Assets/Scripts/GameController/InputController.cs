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

#if UNITY_ANDROID
            GameObject mobileInputPanel = (GameObject) Resources.Load("GUI/MobileInputPanel");
            GameObject mobUI = Instantiate(mobileInputPanel);
            mobUI.GetComponent<MobileInput>().InputController = Instance;
            mobUI.transform.SetParent(GameObject.Find("GUI").transform);
#else
            gameObject.AddComponent<ClassicInput>().InputController = Instance;
#endif

        }

        public void InvokeMoveEvent(float axis)
        {
            if(MoveEvent != null)
                MoveEvent.Invoke(axis);
        }

        public void InvokeCryEvent(float axis)
        {
            if (CryEvent != null)
                CryEvent.Invoke(axis);
        }

        public void InvokeBlockEvent(float axis)
        {
            if (BlockEvent != null)
                BlockEvent.Invoke(axis);
        }

        public void ExecutingCommands()
        {
            while (CommandManager.CountCommands > 0)
            {
                CommandManager.ExecuteNextCommand();
            }
        }
    }
}