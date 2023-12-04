using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Application.Interfaces.KeyboardServiceInterfaces
{
    public interface IReplyKeyboardService
    {
        ReplyKeyboardMarkup CreateKeyboardMarkup(List<string> names, int? rows = 2);
        ReplyKeyboardMarkup CreateLocationRequestKeyboardMarkup(string name);
        ReplyKeyboardMarkup CreateContactRequestKeyboardMarkup(string name);
    }
}
