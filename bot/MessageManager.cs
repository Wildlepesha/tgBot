using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bot
{
    class MessageManager
    {
        private static Dictionary<string, string> messages;

        static MessageManager()
        {
            string json = File.ReadAllText("C:\\Users\\PC\\source\\repos\\bot\\bot\\messages.json");
            messages = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        }

        public static string GetMessage(string key)
        {
            return messages.ContainsKey(key) ? messages[key] : "Текст не найден";
        }
    }

}
