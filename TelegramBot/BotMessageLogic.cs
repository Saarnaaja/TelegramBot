using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    internal class BotMessageLogic
    {
        private Messanger _messanger;
        private Dictionary<long, Conversation> _chatList; 
        
        public BotMessageLogic()
        {
            _messanger = new Messanger();
            _messanger.Inizalize();
            _chatList = new Dictionary<long, Conversation>();
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Событие возникающее при получении сообщения ботом.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task OnUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        await Response(botClient, update.Message);
                        break;
                    }
                case UpdateType.CallbackQuery:
                    {
                        await OnCallback(botClient, update.CallbackQuery);
                        break;
                    }
            }
        }

        /// <summary>
        /// Возвращает беседу <see cref="Conversation">Conversation</see> 
        /// по указанному id в чате <see cref="Chat">Chat</see>.
        /// </summary>
        /// <param name="chat">чат в котором пришло сообщение.</param>
        /// <returns></returns>
        private Conversation GetChat(Chat chat)
        {
            var id = chat.Id;
            if (!_chatList.ContainsKey(id))
            {
                var newchat = new Conversation(chat);

                _chatList.Add(id, newchat);
            }
            return _chatList[id];
        }

        private async Task Response(ITelegramBotClient botClient, Message message)
        {
            if (message == null) return;
            var chat = GetChat(message.Chat);
            chat.AddMessage(message);
            Console.WriteLine($"{message.From.Username}: {message.Text}");
            await _messanger.MakeAnswer(botClient, chat);
        }

        /// <summary>
        /// Событие срабатывающее при нажатии кнопок в боте.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="callbackQuery"></param>
        /// <returns></returns>
        private async Task OnCallback(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery == null) return;
            var chat = GetChat(callbackQuery.Message.Chat);
            Console.WriteLine($"callBack: {callbackQuery.Data}");
            await _messanger.OnCallbackAsync(botClient, chat, callbackQuery);
        }
    }
}
