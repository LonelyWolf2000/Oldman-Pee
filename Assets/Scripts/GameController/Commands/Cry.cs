namespace GameController.Commands
{

    public class Cry : ICommand
    {
        private InputController.ExecuteMethod _CryMethod;

        public Cry(InputController.ExecuteMethod cryMethod)
        {
            _CryMethod = cryMethod;
        }
        public void Execute()
        {
            if (_CryMethod != null)
                _CryMethod.Invoke();
        }
    }
}