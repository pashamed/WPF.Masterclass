using RssReader.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.ViewModel
{
    public class MainVM
    {
        IRssHelper rssHelper;

        public ObservableCollection<Item> Items { get; set; }

        public MainVM(IRssHelper rssHelper)
        {
            Items = new ObservableCollection<Item>();
            this.rssHelper = rssHelper;
            ReadRss();           
        }

        private async void ReadRss()
        {
            var posts = await rssHelper.GetPosts();

            Items.Clear();
            foreach (var post in posts)
            {
                Items.Add(post);
            }
        }
    }
}
