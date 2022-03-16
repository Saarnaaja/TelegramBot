using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Commands
{
    internal interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}
