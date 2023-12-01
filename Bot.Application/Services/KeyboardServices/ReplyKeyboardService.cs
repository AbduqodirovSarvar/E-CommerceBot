using Bot.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Application.Services.KeyboardServices
{
    public class ReplyKeyboardService : IReplyKeyboardService
    {
        public ReplyKeyboardMarkup CreateKeyboardMarkup(List<string> names, int? rows = 2)
        {
            List<KeyboardButton[]> buttonRows = new();
            List<KeyboardButton> buttons = new();
            foreach (var name in names)
            {
                if (buttons.Count == rows)
                {
                    buttonRows.Add(buttons.ToArray());
                    buttons.Clear();
                }
                buttons.Add(new KeyboardButton(name.ToString()));
            }
            if (buttons.Count > 0)
            {
                buttonRows.Add(buttons.ToArray());
            }
            return new ReplyKeyboardMarkup(buttonRows.ToArray()) { ResizeKeyboard = true };
        }

        public ReplyKeyboardMarkup CreateContactRequestKeyboardMarkup(string name)
        {
            var contact = new ReplyKeyboardMarkup(
                          new KeyboardButton(name) { RequestContact = true })
                            { ResizeKeyboard = true };

            return contact;
        }

        public ReplyKeyboardMarkup CreateLocationRequestKeyboardMarkup(string name)
        {
            var location = new ReplyKeyboardMarkup(
                           new KeyboardButton(name) { RequestLocation = true })
                            { ResizeKeyboard = true };

            return location;
        }
    }
}
