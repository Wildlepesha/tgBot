using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using System.Data.SQLite;

namespace bot
{
    class Bot
    {
        static string? corePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
        private static ITelegramBotClient botClient;
        private static readonly string token = File.ReadAllText(@$"{corePath}\secret.txt");
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

            //SQLiteConnection.CreateFile("userdata.sqlite");
            string connectionString = "Data Source=userdata.sqlite;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string expression = "create table if not exists user_info (chatId int CONSTRAINT chatId_unique UNIQUE, surnamename varchar(25), firstname varchar(20), secondname varchar(25), phone int, email varchar(20))";
            SQLiteCommand command = new SQLiteCommand(expression, connection);
            command.ExecuteNonQuery();
            connection.Close();

            await Task.Delay(-1); // Бесконечное ожидание
        }
        public static ITelegramBotClient GetBotClient() => botClient;
    }
}
