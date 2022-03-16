using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Commands
{
    internal class StopTrainingCommand : AbstractCommand, IChatTextCommandWithAction
    {
        public StopTrainingCommand()
        {
            CommandText = "/stop";
            Description = "Остановка тренировки";
        }

        public bool DoAction(Conversation chat)
        {
            chat.IsTraningInProcess = false;
            return !chat.IsTraningInProcess;
        }

        public string GetLastErrorText()
        {
            return "Ошибка выполнения команды";
        }

        public string ReturnText()
        {
            return "Тренировка остановлена!";
        }
    }
}
