namespace GameController.Commands
{
    public class Block : ICommand
    {
        private InputController.ExecuteMethod _BlockMethod;

        public Block(InputController.ExecuteMethod blockMethod)
        {
            _BlockMethod = blockMethod;
        }
        public void Execute()
        {
            if (_BlockMethod != null)
                _BlockMethod.Invoke();
        }
    }
}