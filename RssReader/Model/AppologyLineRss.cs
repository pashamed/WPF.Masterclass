using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RssReader.Model
{
    [XmlRoot(ElementName = "rss")]
    public class AppologyLineRss
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
    }

    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "managingEditor")]
        public string ManagingEditor { get; set; }

        [XmlElement(ElementName = "copyright")]
        public string Copyright { get; set; }

        [XmlElement(ElementName = "generator")]
        public string Generator { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        [XmlElement(ElementName = "language")]
        public string Language { get; set; }

        [XmlElement(ElementName = "itunes:owner")]
        public ItunesOwner ItunesOwner { get; set; }

        [XmlElement(ElementName = "itunes:author")]
        public string ItunesAuthor { get; set; }

        [XmlElement(ElementName = "itunes:summary")]
        public string ItunesSummary { get; set; }

        [XmlElement(ElementName = "itunes:explicit")]
        public string ItunesExplicit { get; set; }

        [XmlElement(ElementName = "itunes:category")]
        public ItunesCategory ItunesCategory { get; set; }

        [XmlElement(ElementName = "itunes:keywords")]
        public string ItunesKeywords { get; set; }

        [XmlElement(ElementName = "itunes:type")]
        public string ItunesType { get; set; }

        [XmlElement(ElementName = "itunes:image")]
        public ItunesImage ItunesImage { get; set; }

        [XmlElement(ElementName = "image")]
        public Image Image { get; set; }

        [XmlElement(ElementName = "item")]
        public List<Item> Items { get; set; }
    }

    public class ItunesOwner
    {
        [XmlElement(ElementName = "itunes:name")]
        public string ItunesName { get; set; }

        [XmlElement(ElementName = "itunes:email")]
        public string ItunesEmail { get; set; }
    }

    public class ItunesCategory
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    public class ItunesImage
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }

    public class Image
    {
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
    }

    public class Item
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "itunes:title")]
        public string ItunesTitle { get; set; }

        [XmlElement(ElementName = "itunes:episodeType")]
        public string ItunesEpisodeType { get; set; }

        [XmlElement(ElementName = "itunes:summary")]
        public string ItunesSummary { get; set; }

        [XmlElement(ElementName = "content:encoded")]
        public string ContentEncoded { get; set; }

        [XmlElement(ElementName = "guid")]
        public string Guid { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }

        [XmlElement(ElementName = "itunes:duration")]
        public string ItunesDuration { get; set; }

        [XmlElement(ElementName = "itunes:image")]
        public ItunesImage ItunesImage { get; set; }

        [XmlElement(ElementName = "itunes:explicit")]
        public string ItunesExplicit { get; set; }

        [XmlElement(ElementName = "itunes:episode")]
        public string ItunesEpisode { get; set; }

        [XmlElement(ElementName = "itunes:season")]
        public string ItunesSeason { get; set; }

        [XmlElement(ElementName = "enclosure")]
        public Enclosure Enclosure { get; set; }
    }

    public class Enclosure
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; }
        [XmlAttribute(AttributeName = "length")]
        public string Length { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }
}

