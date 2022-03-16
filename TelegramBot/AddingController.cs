using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Trainer;

namespace TelegramBot
{
    internal class AddingController
    {
        private Dictionary<long, AddingStage> _stageDictionary;

        public AddingController()
        {
            _stageDictionary = new Dictionary<long, AddingStage>();
        }

        public void AddFirstStage(Conversation chat)
        {
            _stageDictionary.Add(chat.GetId(), AddingStage.Russian);
        }

        public void NextStage(Conversation chat)
        {
            var currentState = _stageDictionary[chat.GetId()];
            _stageDictionary[chat.GetId()] = currentState + 1;

            if (_stageDictionary[chat.GetId()] == AddingStage.Finish)
            {
                chat.IsAdding = false;
                _stageDictionary.Remove(chat.GetId());
            }
        }

        public AddingStage GetStage(Conversation chat)
        {
            return _stageDictionary[chat.GetId()];
        }
    }
}
