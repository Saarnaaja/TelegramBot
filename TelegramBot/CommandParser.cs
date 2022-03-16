using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Commands;

namespace TelegramBot
{
    internal class CommandParser
    {
        private List<IChatCommand> _commands;
        private AddingController _addingController;

        public CommandParser()
        {
            _commands = new List<IChatCommand>();
            _addingController = new AddingController();
        }

        public string ShowCommands()
        {
            var text = string.Empty;
            foreach (AbstractCommand command in _commands)
            {
                text += $"{command.CommandText} - {command.Description}\n";
            }
            return text;
        }

        /// <summary>
        /// Добавление команды в список.
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(IChatCommand command)
        {
            _commands.Add(command);
        }

        /// <summary>
        /// Проверка на существование команды.
        /// </summary>
        /// <param name="message">Текст команды.</param>
        /// <returns></returns>
        public bool IsMessageCommand(string message)
        {
            return _commands.Exists(x => x.CheckMessage(message));
        }

        /// <summary>
        /// Получение текста для ответа на команду <see cref="IChatTextCommand"> IChatTextCommand</see>.
        /// </summary>
        /// <param name="message">Текст команды.</param>
        /// <returns></returns>
        public string GetMessageText(string message, Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IChatTextCommand;
            if (command is IChatTextCommandWithAction commandWithAction)
            {
                if (!commandWithAction.DoAction(chat))
                {
                    return commandWithAction.GetLastErrorText();
                };
            }
            return command.ReturnText();
        }

        /// <summary>
        /// Получение кнопок команды <see cref="IKeyboardCommand"> IKeyboardCommand</see>.
        /// </summary>
        /// <param name="message">Текст команды.</param>
        /// <returns></returns>
        public InlineKeyboardMarkup GetKeyboard(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyboardCommand;
            return command.ReturnKeyboard();
        }

        /// <summary>
        /// Получение текста для ответа на команду <see cref="IKeyboardCommand"> IKeyboardCommand</see>.
        /// </summary>
        /// <param name="message">Текст команды.</param>
        /// <returns></returns>
        public string GetInformationalMessage(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyboardCommand;
            return command.InformationalMessage();
        }

        /// <summary>
        /// Обработка события <see cref="CallbackQuery"> CallbackQuery</see> у команды.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="message"></param>
        /// <param name="callbackQuery"></param>
        /// <returns></returns>
        public async Task RunCallbackAsync(ITelegramBotClient botClient, Conversation chat, string message, CallbackQuery callbackQuery)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyboardCommand;
            await command?.OnCallbackAsync(botClient, chat, callbackQuery);
        }

        public bool IsTextCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));
            return command is IChatTextCommand;
        }

        public bool IsButtonCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));
            return command is IKeyboardCommand;
        }

        public bool IsAddingCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));
            return command is AddWordCommand;
        }

        public void StartAddingWord(ITelegramBotClient botClient, string message,  Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as AddWordCommand;
            _addingController.AddFirstStage(chat);
            command.StartProcessAsync(botClient, chat);
        }

        public void NextStage(ITelegramBotClient botClient, string message, Conversation chat)
        {
            var command = _commands.Find(x => x is AddWordCommand) as AddWordCommand;
            command.NextStageAsync(botClient, _addingController.GetStage(chat), chat, message);
            _addingController.NextStage(chat);
        }

        public void ContinueTraining(ITelegramBotClient botClient, string message, Conversation chat)
        {
            var command = _commands.Find(x => x is TrainingCommand) as TrainingCommand;
            command.NextStepAsync(botClient, chat, message);
        }

        public bool IsShowWordCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));
            return command is ShowWordsCommand;
        }
    }
}
