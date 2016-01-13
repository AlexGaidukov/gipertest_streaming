using System;
using System.Collections;

namespace VisualEditor.Logic.Commands
{
    internal class CommandManager
    {
        private static CommandManager instance;
        private readonly Hashtable commands;

        private CommandManager()
        {
            commands = new Hashtable();
        }

        public static CommandManager Instance
        {
            get
            {
                return instance ?? (instance = new CommandManager());
            }
        }

        public void Register(AbstractCommand command)
        {
            if (commands.ContainsKey(command.Name))
            {
                commands[command.Name] = command;
            }
            else
            {
                commands.Add(command.Name, command);
            }
        }

        public AbstractCommand GetCommand(string name)
        {
            if (commands.ContainsKey(name))
            {
                return (AbstractCommand)commands[name];
            }

            throw new InvalidOperationException();
        }
    }
}