using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Trainer;

namespace TelegramBot.Commands
{
    internal class TrainingCommand : AbstractCommand, IKeyboardCommand
    {

        private Dictionary<long, TrainingType> _training;

        private Dictionary<long, string> _activeWord;

        public TrainingCommand()
        {
            CommandText = "/training";
            Description = "Запуск тренировки";
            _training = new Dictionary<long, TrainingType>();
            _activeWord = new Dictionary<long, string>();
        }

        public InlineKeyboardMarkup ReturnKeyboard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton("С русского на английский")
                {
                    CallbackData = "rustoeng"
                },

                new InlineKeyboardButton("С английского на русский")
                {
                    CallbackData = "engtorus"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        public string InformationalMessage()
        {
            return "Выберите тип тренировки. Для окончания тренировки введите команду /stop";
        }

        public async Task OnCallbackAsync(ITelegramBotClient botClient, Conversation chat, CallbackQuery e)
        {
            var text = "";
            var id = e.Message.Chat.Id;
            if (chat.WordsDictionary.Count > 0)
            {
                if (chat.IsTraningInProcess)
                {
                    text = "Тренировка уже идет";
                }
                else
                {
                    chat.IsTraningInProcess = true;
                    switch (e.Data)
                    {
                        case "rustoeng":
                            {
                                _training.Add(id, TrainingType.RusToEng);
                                text = chat.GetTrainingWord(TrainingType.RusToEng);
                                break;
                            }
                        case "engtorus":
                            {
                                _training.Add(id, TrainingType.EngToRus);
                                text = chat.GetTrainingWord(TrainingType.EngToRus);
                                break;
                            }
                        default:
                            break;
                    }
                    _activeWord.Add(id, text);
                }
            }
            else
            {
                chat.IsTraningInProcess = false;
                text = "Словарь пуст.\nСначала необходимо добавить слова через команду /addword";
            }
            await botClient.SendTextMessageAsync(id, text);
            await botClient.AnswerCallbackQueryAsync(e.Id);
        }

        public async void NextStepAsync(ITelegramBotClient botClient, Conversation chat, string message)
        {
            var type = _training[chat.GetId()];
            var word = _activeWord[chat.GetId()];
            var check = chat.CheckWord(type, word, message);

            var text = (check ? "Правильно!" : "Неправильно!") + " Следующее слово: ";
            var newword = chat.GetTrainingWord(type);

            text += newword;
            _activeWord[chat.GetId()] = newword;

            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }
    }
}
