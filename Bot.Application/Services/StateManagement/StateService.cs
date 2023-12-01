using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Services.StateManagement
{
    public static class StateService
    {
        private static readonly string[] states =
        {
            "register:language", // 1
            "register:fullname", // 2
            "register:phone", // 3
            "feedback", // 4
            "contact", // 5
            "information", // 6
            "information:filial", // 7
            "setting", // 8
            "setting:changename", // 9
            "setting:changephone", // 10
            "setting:changelanguage", // 11
            "order", // 12
            "order:delivery", // 13
            "order:takeaway", // 14
            "order:delivery:address", // 15
            "order:delivery:confirmationaddress" // 16
        };

        private static readonly Dictionary<long, string?> userStates = new();

        internal static Task Set(long key, string value)
        {
            if (userStates.ContainsKey(key))
            {
                userStates[key] = value;
            }
            else
            {
                userStates.Add(key, value);
            }
            return Task.CompletedTask;
        }

        internal static string? Get(long key)
        {
            if (userStates.TryGetValue(key, value: out string? value))
            {
                return value;
            }
            return null;
        }

        internal static Task Delete(long key)
        {
            if (userStates.ContainsKey(key))
            {
                userStates.Remove(key);
            }
            return Task.CompletedTask;
        }


    }
}
