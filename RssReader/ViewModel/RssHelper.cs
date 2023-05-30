using RssReader.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RssReader.ViewModel
{
    public class RssHelper
    {

        public async static Task<List<Item>> GetPosts()
        {
            List<Item>? posts = new List<Item>();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppologyLineRss));

            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("https://rss.art19.com/apology-line");

                using (StringReader stream = new StringReader(response))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppologyLineRss));
                    AppologyLineRss f = (AppologyLineRss)serializer.Deserialize(stream);
                    posts = f?.Channel.Items;
                }
            }
            return posts;
        }
    }
}
