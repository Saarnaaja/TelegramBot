using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Commands
{
    internal class ShowWordsCommand : AbstractCommand
    {
        public ShowWordsCommand()
        {
            CommandText = "/dictionary";
            Description = "Вывод всех слов из словаря";
        }
    }
}
