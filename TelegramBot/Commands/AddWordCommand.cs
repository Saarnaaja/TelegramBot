using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Trainer;

namespace TelegramBot.Commands
{
    internal class AddWordCommand : AbstractCommand
    {
        private Dictionary<long, Word> _tempDictionary;

        public AddWordCommand()
        {
            CommandText = "/addword";
            Description = "Добавление нового слова в словарь";
            _tempDictionary = new Dictionary<long, Word>();
        }

        public async void StartProcessAsync(ITelegramBotClient botClient, Conversation chat)
        {
            _tempDictionary.Add(chat.GetId(), new Word());
            var text = "Введите русское значение слова";
            await SendCommandText(botClient, text, chat.GetId());
        }

        public async void NextStageAsync(ITelegramBotClient botClient, AddingStage state, Conversation chat, string message)
        {
            var word = _tempDictionary[chat.GetId()];
            var text = string.Empty;

            switch (state)
            {
                case AddingStage.Russian:
                    {
                        word.Russian = message;
                        if (chat.WordsDictionary.ContainsKey(word.Russian))
                        {
                            text = "Данное слово уже есть в словаре.";
                            chat.IsAdding = false;
                        }
                        else
                        {
                            text = "Введите английское значение слова";
                        }
                        break;
                    }
                case AddingStage.English:
                    {
                        word.English = message;
                        text = "Введите тематику";
                        break;
                    }
                case AddingStage.Theme:
                    {
                        word.Theme = message;
                        text = "Успешно! Слово " + word.English + " добавлено в словарь. ";
                        chat.WordsDictionary.Add(word.Russian, word);
                        _tempDictionary.Remove(chat.GetId());

                        break;
                    }
            }
            await SendCommandText(botClient, text, chat.GetId());
        }

        private async Task SendCommandText(ITelegramBotClient botClient, string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }
    }
}
