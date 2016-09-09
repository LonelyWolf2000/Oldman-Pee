using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameController.Commands;

namespace GameController
{
    public static class CommandManager
    {
        public static int CountCommands { get; private set; }
        private static Queue<ICommand> _commands;

        public static void RegisterCommand(ICommand command)
        {
            if(_commands == null)
                _commands = new Queue<ICommand>();

            if (command != null)
            {
                _commands.Enqueue(command);
                CountCommands = _commands.Count;
            }
        }

        public static void ExecuteNextCommand()
        {
            if (_commands != null && _commands.Count > 0)
            {
                _commands.Dequeue().Execute();
                CountCommands = _commands.Count;
            }
        }
    }
}