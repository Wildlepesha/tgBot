﻿using bot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Program
{
    class Program
    {
        static async Task Main()
        {
            await Bot.Start();
        }

    }

}
