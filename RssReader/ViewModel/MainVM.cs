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
        public ObservableCollection<Item> Items { get; set; }

        public MainVM()
        {
            Items = new ObservableCollection<Item>();
            ReadRss();
        }

        private async void ReadRss()
        {
            var posts = await RssHelper.GetPosts();

            Items.Clear();
            foreach (var post in posts)
            {
                Items.Add(post);
            }
        }
    }
}
