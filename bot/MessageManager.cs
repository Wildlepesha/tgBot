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
        private static Dictionary<string, string>? messages;

        static MessageManager()
        {
            string? corePath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
            string json = File.ReadAllText(@$"{corePath}\messages.json");
            messages = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static string GetMessage(string key)
        {
            string? placeholder;
            try
            {
                return messages.TryGetValue(key, out placeholder) ? messages[key] : placeholder = "Текст не найден";
            } catch (NullReferenceException ex)
            {
                throw ex;
            }
        }
    }
}
