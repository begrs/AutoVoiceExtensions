using System;
using System.Collections.Generic;

namespace ave_server.Core
{
    public class CommandTable : IDisposable
    {
        private IReadOnlyList<CommandItem>? _commands;

        public IReadOnlyList<CommandItem> ReadCommands()
        {
            if (_commands is null)
                _commands = readCommands();
            return _commands;
        }
        private IReadOnlyList<CommandItem> readCommands()
        {
            var list = new List<CommandItem>(){
                new CommandItem("auto-voice-ave.activate","AVE (Voice): Activate/Deactivate"),
                new CommandItem("auto-voice-ave.listen","AVE (Voice): Listen"),
                new CommandItem("dummy1.cmd","Dummy 1"),
            };

            return list.AsReadOnly();
        }

        public void Dispose()
        {
            _commands = null;
        }
    }

    public record CommandItem(string Command, string HumanReadable);
}