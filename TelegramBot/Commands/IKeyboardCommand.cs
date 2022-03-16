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
    internal interface IKeyboardCommand
    {
        string InformationalMessage();
        InlineKeyboardMarkup ReturnKeyboard();
        Task OnCallbackAsync(ITelegramBotClient botClient, Conversation chat, CallbackQuery e);
    }
}
