using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Commands
{
    internal class DeleteWordCommand : ChatTextCommandOption, IChatTextCommandWithAction
    {
        private string _lastError;
        public DeleteWordCommand()
        {
            CommandText = "/deleteword";
            Description = "Удаление слова из словаря.\nПример: /deleteword ваше_слово";
        }

        public bool DoAction(Conversation chat)
        {
            var message = chat.GetLastMessage();

            var text = ClearMessageFromCommand(message.Text);

            if (text.Length == 0)
            {
                _lastError = "Необходимо указать слово для удаления.";
                return false;
            }

            if (chat.WordsDictionary.ContainsKey(text))
            {
                chat.WordsDictionary.Remove(text);
                return true;
            }
            _lastError = $"Слово \"{text}\" не найдено в словаре.";
            return false;
        }

        public string GetLastErrorText()
        {
            return _lastError;
        }

        public string ReturnText()
        {
            return "Слово успешно удалено!";
        }


    }
}
