using RssReader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.ViewModel
{
    public interface IRssHelper
    {
        public Task<List<Item>> GetPosts();
    }
}
