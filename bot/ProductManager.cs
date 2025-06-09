using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace bot
{
    class ProductManager
    {
        static Dictionary<long, List<int>> userAlbums = new();
        static ProductManager()
        {

        }

        //public static YmlCatalog ParseYaml()
        //{
        //    // Определяем YAML-строку
        //    string yml = @"C:\Users\PC\Downloads\Telegram Desktop\filtered_bouquets_v1_only (2).yml";
        //    var catalog = YmlDeserializer.Deserialize(yml);
            
        //    return catalog;
        //}

        //public static List<List<Dictionary<string, string>>> GetProductsList()
        //{
        //    /*
        //     * [
        //     *  p1 [ o1 { key; value, key; value } o2 { key; value, key; value } ],
        //     *  p2 [ o1 { key; value, key; value } o2 { key; value, key; value } ]
        //     * ]
        //    */

        //    List<List<Dictionary<string, string>>> pages = new List<List<Dictionary<string, string>>>();
        //    List<Dictionary<string, string>> _tmp = new List<Dictionary<string, string>>();
        //    Dictionary<string, string> _tmpDic;
        //    var offers = ParseYaml().Shop.Offers;
        //    Dictionary<string, string> picturesUrls = new Dictionary<string, string>();

        //    for (int i = 1; i < offers.Count + 1; i++)
        //    {
        //        _tmpDic = new();
        //        _tmpDic.Add("id", i.ToString());
        //        _tmpDic.Add("picture", offers[i - 1].Picture);
        //        _tmpDic.Add("name", offers[i - 1].Name);
        //        _tmpDic.Add("price", offers[i - 1].Price.ToString());
        //        _tmp.Add(_tmpDic);
        //        if (i % 3 == 0)
        //        {
        //            pages.Add(_tmp);
        //            _tmp = new();
        //        }
        //        else if (i == offers.Count)
        //        {
        //            pages.Add(_tmp);
        //        }

        //    }

        //    return pages;
        //}

        //public static int TotalPages => GetProductsList().Count(); 

        public static List<Dictionary<string, string>> GetProductsByPage(List<List<Dictionary<string, string>>> pages, int page)
        {
            if (page < 1 || page > pages.Count) return new List<Dictionary<string, string>>();
            return pages[page - 1];
        }

        public static string GetInfoFromPageElement(List<Dictionary<string, string>> page, string key, int elem)
        {
            if (elem !< 1 || elem !> 3)
            {
                return page[elem - 1][key];
            }
            return null;
        }

        public static void SaveUserAlbum(long chatId, List<int> messageIds)
        {
            if (userAlbums.ContainsKey(chatId))
            {
                userAlbums[chatId] = messageIds;
            }
            else
            {
                userAlbums.Add(chatId, messageIds);
            }
        }
        public static List<int> GetUserAlbum(long chatId)
        {
            return userAlbums.ContainsKey(chatId) ? userAlbums[chatId] : new List<int>();
        }
    }
}
