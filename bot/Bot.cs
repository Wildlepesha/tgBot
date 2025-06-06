using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;

namespace bot
{
    class Bot
    {
        private static ITelegramBotClient botClient;
        private static readonly string token = "";
        public static async Task Start()
        {
            var botClient = new TelegramBotClient(token);

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // Получаем все типы обновлений
            };

            botClient.StartReceiving(
                UpdateHandler.HandleUpdateAsync,
                UpdateHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );

            var botInfo = await botClient.GetMe();
            Console.WriteLine($"Бот {botInfo.Username} запущен...");

            await Task.Delay(-1); // Бесконечное ожидание
        }
        public static ITelegramBotClient GetBotClient() => botClient;
    }
}
