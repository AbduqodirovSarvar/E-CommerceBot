using Bot.Application.Interfaces;
using Bot.Application.Interfaces.KeyboardServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Application.Services.KeyboardServices
{
    public class InlineKeyboardService : IInlineKeyboardService
    {
        public InlineKeyboardMarkup CreateKeyboardMarkup(Dictionary<string, string> options, int? rows = 2)
        {
            List<InlineKeyboardButton[]> buttonRows = new();
            List<InlineKeyboardButton> buttons = new();
            foreach (var item in options)
            {
                if (buttons.Count == rows)
                {
                    buttonRows.Add(buttons.ToArray());
                    buttons.Clear();
                }
                buttons.Add(InlineKeyboardButton.WithCallbackData(item.Key.ToString(), item.Value.ToLower()));
            }
            if (buttons.Count > 0)
            {
                buttonRows.Add(buttons.ToArray());
            }
            return new InlineKeyboardMarkup(buttonRows.ToArray());
        }
    }
}
