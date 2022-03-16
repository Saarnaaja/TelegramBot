using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.Trainer;

namespace TelegramBot
{
    internal class Conversation
    {
        private Chat _telegramChat;
        private List<Message> _telegramMessages;

        public Dictionary<string, Word> WordsDictionary;
        public bool IsAdding;
        public bool IsTraningInProcess;

        public long GetId() => _telegramChat.Id;

        public Message GetLastMessage()
        {
            var index = _telegramMessages.Count - 1;
            return index >= 0 ? _telegramMessages[_telegramMessages.Count - 1] : null;
        }

        public Conversation(Chat chat)
        {
            _telegramChat = chat;
            _telegramMessages = new List<Message>();
            WordsDictionary = new Dictionary<string, Word>();
        }

        public void AddMessage(Message message)
        {
            _telegramMessages.Add(message);
        }

        public string GetTrainingWord(TrainingType type)
        {
            var rand = new Random();
            var item = rand.Next(0, WordsDictionary.Count);
            var randomword = WordsDictionary.Values.AsEnumerable().ElementAt(item);
            var text = string.Empty;

            switch (type)
            {
                case TrainingType.EngToRus:
                    text = randomword.English;
                    break;

                case TrainingType.RusToEng:
                    text = randomword.Russian;
                    break;
            }

            return text;
        }

        public bool CheckWord(TrainingType type, string word, string answer)
        {
            Word control;
            var result = false;

            switch (type)
            {

                case TrainingType.EngToRus:
                    {
                        control = WordsDictionary.Values.FirstOrDefault(x => x.English == word);
                        result = control.Russian == answer;
                        break;
                    }
                case TrainingType.RusToEng:
                    {
                        control = WordsDictionary[word];
                        result = control.English == answer;
                        break;
                    }
            }

            return result;
        }
    }
}
