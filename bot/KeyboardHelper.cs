using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot
{
    class KeyboardHelper
    {
        public static InlineKeyboardButton[] menuMarkup = new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад в меню", "mainMenu")
            };

        public static InlineKeyboardMarkup GetMainMenu()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
            {
                InlineKeyboardButton.WithCallbackData("Заказать сборный букет", "orderCombo"),

            },
                new[]
            {
                InlineKeyboardButton.WithCallbackData("Заказать моно букет", "orderMono")
            },
                new[]
            {
                InlineKeyboardButton.WithCallbackData("Заказать композицию", "orderComp")
            },
                new[]
            {
                InlineKeyboardButton.WithCallbackData("Собрать свой букет", "orderSelf")
            }

        });
        }
        public static InlineKeyboardMarkup NavBar(int currentPage)
        {
            int totalPages = ProductManager.TotalPages;
            var products = ProductManager.GetProductsList();
            var productsPage = ProductManager.GetProductsByPage(products, currentPage);
            InlineKeyboardMarkup buttons = new InlineKeyboardMarkup();

            if (currentPage > 1)
            {
                buttons.AddButton(InlineKeyboardButton.WithCallbackData("Назад", $"photo_{currentPage - 1}"));
                
            }
            if (currentPage < totalPages)
            {
                buttons.AddButton(InlineKeyboardButton.WithCallbackData("Вперед", $"photo_{currentPage + 1}"));
            }

            foreach (var product in productsPage)
            {
                buttons.AddNewRow(InlineKeyboardButton.WithCallbackData($"{product["name"]} {product["price"]}", $"product_{product["id"]}"));
               
            }
            buttons.AddNewRow(menuMarkup);

            return buttons;
        }
        public static InlineKeyboardMarkup GetMenu()
        {
            return new InlineKeyboardMarkup(new[]
            {
                menuMarkup,
            });
        }
        public static InlineKeyboardMarkup OrderStepper(int step)
        {
        return new InlineKeyboardMarkup(new[]
        {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Назад", "back"),
            InlineKeyboardButton.WithCallbackData($"Далее {step}", $"orderStep_{step}"),

        },
    });
    }
    }
}
