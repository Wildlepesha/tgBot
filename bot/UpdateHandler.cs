using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bot;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace bot
{
    class UpdateHandler
    {
        public static int step = 1;
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
            else
            {
                await botClient.SendMessage(chatId, "Я не понимаю. Нажми кнопку ниже.", replyMarkup: KeyboardHelper.GetMainMenu());
            }
        }

        private static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            long chatId = callbackQuery.Message.Chat.Id;
            string data = callbackQuery.Data;
            

            if (data == "orderSelf")
            {
                await botClient.EditMessageText(chatId, callbackQuery.Message.Id, "Телефон для связи с флористом +89092022121", replyMarkup: KeyboardHelper.GetMenu());
            }
            else if (data == "mainMenu")
            {
                await botClient.EditMessageText(chatId, callbackQuery.Message.Id, MessageManager.GetMessage("start"), replyMarkup: KeyboardHelper.GetMainMenu());
               
            }
            else if (data == "orderCombo")
            {
                await SendAlbum(botClient, chatId, 1);
            }
            else if (data == "otherDate")
            {

            }
            else if (data == "back")
            {
                await botClient.SendMessage(
                                    chatId,
                                    MessageManager.GetMessage($"orderStep_{step}"),
                                    replyMarkup: KeyboardHelper.GetMainMenu()
                                );
                if (step > 1)
                {
                    step -= 1;
                }
            }
            else if (data.Contains("orderStep_"))
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

            if (data.StartsWith("photo_"))
            {
                int page = int.Parse(data.Split('_')[1]);
                await SendAlbum(botClient, chatId, page);
            }

            await botClient.AnswerCallbackQuery(callbackQuery.Id);
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return Task.CompletedTask;
        }

        static async Task SendAlbum(ITelegramBotClient botClient, long chatId, int page)
        {
            var pages = ProductManager.GetProductsList();
            var products = ProductManager.GetProductsByPage(pages, page);
            var oldMessages = ProductManager.GetUserAlbum(chatId);

            foreach (var message in oldMessages)
            {
                try
                {
                    await botClient.DeleteMessage(chatId, message);
                } catch { }
                
            }

            List<InputMediaPhoto> media = new();

            foreach (var product in products)
            {
                media.Add(new InputMediaPhoto(product["picture"]));
            }
            List<int> newMessageIds = new();
            var messages = await botClient.SendMediaGroup(chatId, media);

            foreach (var msg in messages)
            {
                newMessageIds.Add(msg.MessageId);
            }
            var navMessage = await botClient.SendMessage(chatId, $"Страница {page}/{ProductManager.TotalPages}", replyMarkup: KeyboardHelper.NavBar(page));
            
            newMessageIds.Add(navMessage.MessageId);
            ProductManager.SaveUserAlbum(chatId, newMessageIds);
            
        }
    }
}


