using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Commands
{
    internal abstract class ChatTextCommandOption : AbstractCommand
    {
        public override bool CheckMessage(string message)
        {
            if (message == null) return false;
            return message.StartsWith(CommandText);
        }

        public string ClearMessageFromCommand(string message)
        {
            if (message.Length < CommandText.Length + 1) return "";
            return message.Substring(CommandText.Length + 1);
        }

    }
}
