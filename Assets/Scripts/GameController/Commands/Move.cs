namespace GameController.Commands
{
    public class Move : ICommand
    {
        private InputController.ExecuteMethod _MoveMethod;

        public Move(InputController.ExecuteMethod moveMethod)
        {
            _MoveMethod = moveMethod;
        }
        public void Execute()
        {
            if(_MoveMethod != null)
                _MoveMethod.Invoke();
        }
    }
}
