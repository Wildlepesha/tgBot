using bot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Data.SQLite;
using System.Data;

namespace bot
{
    class UpdateHandler
    {
        public static int step = 1;
        public static bool orderInProgress = false;

        public static void sqlManage(string expression)
        {
            string connectionString = "Data Source=userdata.sqlite;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(expression, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { } message)
            {
                await HandleMessage(botClient, message);
            }
            else if (update.CallbackQuery is { } callbackQuery)
            {
                await HandleCallbackQuery(botClient, callbackQuery);
            }
        }

        private static async Task HandleMessage(ITelegramBotClient botClient, Message message)
        {
            long chatId = message.Chat.Id;
            Console.WriteLine($"Получено сообщение: {message.Text}");

            if (message.Text == "/start")
            {
                await botClient.SendMessage(
                    chatId,
                    MessageManager.GetMessage("start"),
                    replyMarkup: KeyboardHelper.GetMainMenu()
                );
            }
            else if (orderInProgress && message.Text != null)
            {
                await botClient.SendMessage(
                    chatId,
                    message.Text
                );
                string[] data = message.Text.Split(" ");
                string expression = $"insert into user_info (chatId, surnamename, firstname, secondname, phone, email) values ({message.Chat.Id}, '{data[0]}', '{data[1]}', '{data[2]}', {data[3]}, '{data[4]}');";
                try
                {
                    sqlManage(expression);
                } catch (SQLiteException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message} запуск изменения данных");
                    expression = $"update user_info set surnamename = '{data[0]}', firstname = '{data[1]}', secondname = '{data[2]}', phone = {data[3]}, email = '{data[4]}' where chatId = {message.Chat.Id};";
                    sqlManage(expression);
                }
            }
            else
            {
                await botClient.SendMessage(chatId, "Я не понимаю. Нажми кнопку ниже.", replyMarkup: KeyboardHelper.ResetMenu());
            }
        }

        private static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            long? chatId = callbackQuery.Message.Chat.Id;
            string? data = callbackQuery.Data;
            
            switch (data) {
                case "mainMenu":
                    await botClient.EditMessageText(chatId, callbackQuery.Message.Id, MessageManager.GetMessage("start"), replyMarkup: KeyboardHelper.GetMainMenu());
                    break;
                case "otherDate":
                    break;
                case "orderCombo":
                    break;
                case "orderMono":
                    orderInProgress = true;
                    await botClient.EditMessageText(chatId, callbackQuery.Message.Id, MessageManager.GetMessage("orderForm"), replyMarkup: KeyboardHelper.GetMainMenu());
                    break;
                case "orderComp":
                    break;
                case "orderSelf":
                    await botClient.EditMessageText(chatId, callbackQuery.Message.Id, MessageManager.GetMessage("managerPhone"), replyMarkup: KeyboardHelper.GetMenu());
                    break;
                case "back":
                    await botClient.SendMessage(
                                    chatId,
                                    MessageManager.GetMessage($"orderStep_{step}"),
                                    replyMarkup: KeyboardHelper.GetMainMenu()
                                );
                    if (step > 1)
                    {
                        step -= 1;
                    }
                    break;
                default:
                    break;
            }
            
            if (data.Contains("orderStep_"))
            {
                var steps = data.Split("_");
                if (steps[1] == "1")
                {
                    await botClient.SendMessage(chatId, MessageManager.GetMessage($"orderStep_{step}"), replyMarkup: KeyboardHelper.OrderStepper(step));
                    step += 1;
                }
                else if (steps[1] == "2")
                {
                    await botClient.SendMessage(chatId, MessageManager.GetMessage($"orderStep_{step}"), replyMarkup: KeyboardHelper.OrderStepper(step));
                    step += 1;
                }
            }
            //else if (data.StartsWith("photo_"))
            //{
            //    int page = int.Parse(data.Split('_')[1]);
            //    await SendAlbum(botClient, chatId, page);
            //}
            await botClient.AnswerCallbackQuery(callbackQuery.Id);
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return Task.CompletedTask;
        }

        //static async Task SendAlbum(ITelegramBotClient botClient, long chatId, int page)
        //{
        //    var pages = ProductManager.GetProductsList();
        //    var products = ProductManager.GetProductsByPage(pages, page);
        //    var oldMessages = ProductManager.GetUserAlbum(chatId);

        //    foreach (var message in oldMessages)
        //    {
        //        try
        //        {
        //            await botClient.DeleteMessage(chatId, message);
        //        } catch { }
                
        //    }

        //    List<InputMediaPhoto> media = new();

        //    foreach (var product in products)
        //    {
        //        media.Add(new InputMediaPhoto(product["picture"]));
        //    }
        //    List<int> newMessageIds = new();
        //    var messages = await botClient.SendMediaGroup(chatId, media);

        //    foreach (var msg in messages)
        //    {
        //        newMessageIds.Add(msg.MessageId);
        //    }
        //    var navMessage = await botClient.SendMessage(chatId, $"Страница {page}/{ProductManager.TotalPages}", replyMarkup: KeyboardHelper.NavBar(page));
            
        //    newMessageIds.Add(navMessage.MessageId);
        //    ProductManager.SaveUserAlbum(chatId, newMessageIds);
            
        //}
    }
}


