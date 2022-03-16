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
    internal class Messanger
    {
        private CommandParser _parser;

        public Messanger()
        {
            _parser = new CommandParser();
        }

        public void Inizalize()
        {
            _parser.AddCommand(new SayHiCommand());
            _parser.AddCommand(new PoemButtonCommand());
            _parser.AddCommand(new AddWordCommand());
            _parser.AddCommand(new DeleteWordCommand());
            _parser.AddCommand(new TrainingCommand());
            _parser.AddCommand(new StopTrainingCommand());
            _parser.AddCommand(new ShowWordsCommand());
        }

        /// <summary>
        /// Метод реализующий логику обработки входящих боту сообщений.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        public async Task MakeAnswer(ITelegramBotClient botClient, Conversation chat)
        {
            var lastMessage = chat.GetLastMessage();
            if (lastMessage == null) return;
            var chatid = lastMessage.Chat.Id;

            if (chat.IsTraningInProcess && !_parser.IsTextCommand(lastMessage.Text))
            {
                _parser.ContinueTraining(botClient, lastMessage.Text, chat);
                return;
            }
            if (chat.IsAdding)
            {
                _parser.NextStage(botClient, lastMessage.Text, chat);
                return;
            }
            if (_parser.IsMessageCommand(lastMessage.Text))
            {
                await ExecCommand(botClient, chat, lastMessage.Text);
            }
            else
            {
                var text = CreateTextMessage();
                await botClient.SendTextMessageAsync(chatId: chatid, text: text);
            }
        }

        /// <summary>
        /// Метод реализующий логику обработки нажатия кнопок.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="chat"></param>
        /// <param name="callbackQuery"></param>
        /// <returns></returns>
        public async Task OnCallbackAsync(ITelegramBotClient botClient, Conversation chat, CallbackQuery callbackQuery)
        {
            var message = chat.GetLastMessage();
            if (_parser.IsButtonCommand(message?.Text))
            {
                await _parser.RunCallbackAsync(botClient, chat, message.Text, callbackQuery);
            }
        }

        private async Task ExecCommand(ITelegramBotClient botClient, Conversation chat, string command)
        {
            if (_parser.IsTextCommand(command))
            {
                var text = _parser.GetMessageText(command, chat);
                await SendText(botClient, chat, text);
            }
            else if (_parser.IsButtonCommand(command))
            {
                var keyboard = _parser.GetKeyboard(command);
                var text = _parser.GetInformationalMessage(command);
                await SendTextWithKeyboard(botClient, chat, text, keyboard);
            }
            else if (_parser.IsAddingCommand(command))
            {
                chat.IsAdding = true;
                _parser.StartAddingWord(botClient, command, chat);
            }
            else if (_parser.IsShowWordCommand(command))
            {
                var text = CreateWordListMessage(chat);
                await SendText(botClient, chat, text);
            }
        }

        private async Task SendText(ITelegramBotClient botClient, Conversation chat, string text)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

        private async Task SendTextWithKeyboard(ITelegramBotClient botClient, Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }

        private string CreateTextMessage()
        {
            var text = $"Неизвестная команда.\n{_parser.ShowCommands()}";
            return text;
        }

        private string CreateWordListMessage(Conversation chat)
        {
            if (chat.WordsDictionary.Count == 0) 
                return "Словарь пуст.\nДля добавления слов используйте команду /addword";
            var text = "Словарь: ";
            var sorted = chat.WordsDictionary.OrderBy(x => x.Key);
            foreach (var word in sorted)
            {
                text += $"\n{word.Key} - {word.Value.English}";
            }
            return text;
        }
    }
}
