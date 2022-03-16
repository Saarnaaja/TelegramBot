using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Commands
{
    internal abstract class AbstractCommand : IChatCommand
    {
        public string CommandText;
        public string Description;

        public virtual bool CheckMessage(string message)
        {
            return CommandText == message;
        }
    }
}
