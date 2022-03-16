using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Commands
{
    internal class SayHiCommand : AbstractCommand, IChatTextCommand
    {
        public SayHiCommand()
        {
            CommandText = "/saymehi";
            Description = "Сказать \"Привет\"";
        }

        public string ReturnText()
        {
            return "Привет";
        }
    }
}
