using RssReader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.ViewModel
{
    public class FakeRssHelper : IRssHelper
    {
        public async Task<List<Item>> GetPosts()
        {            
            return await Task.Run(() =>
            {
                var posts = new List<Item>
                {
                    new Item
                    {
                        Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam pulvinar eget ex eu imperdiet. Aliquam porta vehicula leo vitae fermentum. Etiam tempus sed sem vel rhoncus. Maecenas quis lectus eleifend, bibendum orci tincidunt, consequat nisi. Maecenas hendrerit sit amet arcu dignissim blandit. Pellentesque sed malesuada metus. Phasellus venenatis nisi sit.",
                        PubDate = "Thu, 22 Nov 2022 6:13:30 GMT"
                    },

                    new Item
                    {
                        Title = "",
                        PubDate = "Thu, 23 Nov 2022 6:13:30 GMT"
                    }
                };
                return posts;
            });
        }
    }
}
