using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;

namespace TelegramBot
{
    public class BotWorker
    {
        private ITelegramBotClient _botClient;
        private BotMessageLogic _logic;
        private CancellationTokenSource _cts;

        public void Inizalize()
        {
            _botClient = new TelegramBotClient(BotCredentials.BotToken);
            _logic = new BotMessageLogic();
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            ReceiverOptions receiverOptions = new ReceiverOptions() { AllowedUpdates = { } };
            _botClient.StartReceiving(_logic.OnUpdateAsync, _logic.HandleErrorAsync, receiverOptions, _cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
        }
    }
}
