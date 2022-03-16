using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Commands
{
    internal class PoemButtonCommand : AbstractCommand, IKeyboardCommand
    {
        public PoemButtonCommand()
        {
            CommandText = "/poem";
            Description = "Выбор поэта через Inline-кнопки";
        }

        public string InformationalMessage()
        {
            return "Выберите поэта";
        }

        public InlineKeyboardMarkup ReturnKeyboard()
        {
            var buttonList = new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton("Пушкин")
                {
                    CallbackData = "pushkin",
                },
                new InlineKeyboardButton("Есенин")
                {
                    CallbackData = "esenin"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);
            return keyboard;
        }

        public async Task OnCallbackAsync(ITelegramBotClient botClient, Conversation chat, CallbackQuery e)
        {
            var text = "";

            switch (e.Data)
            {
                case "pushkin":
                    text = @"Я помню чудное мгновенье:
                                    Передо мной явилась ты,
                                    Как мимолетное виденье,
                                    Как гений чистой красоты.";
                    break;
                case "esenin":
                    text = @"Не каждый умеет петь,
                                Не каждому дано яблоком
                                Падать к чужим ногам.";
                    break;
                default:
                    break;
            }

            await botClient.SendTextMessageAsync(chat.GetId(), text);
            await botClient.AnswerCallbackQueryAsync(e.Id);
        }
    }
}
