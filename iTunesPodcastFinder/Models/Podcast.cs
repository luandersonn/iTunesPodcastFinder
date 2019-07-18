using iTunesPodcastFinder.Helpers;
using System;
using System.Text.RegularExpressions;

namespace iTunesPodcastFinder.Models
{
    public class Podcast : IPodcast
    {
        public string Name { get; internal set; }
        public string Summary { get; internal set; }
        public string Editor { get; internal set; }
        public string ItunesLink { get; internal set; }
        public string ItunesId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ItunesLink))
                    return null;
                return Regex.Match(ItunesLink, @"/id(?<ID>(\d)+)").Groups["ID"].Value;
            }
        }
        public string FeedUrl { get; internal set; }
        public string Website {get; internal set;}
        public DateTime ReleaseDate { get; internal set; }
        public int EpisodesCount { get; internal set; }
        public string ArtWork { get; internal set; }
        public string Genre { get; internal set; }
        public string InnerXml { get; internal set; }
        public FeedType? FeedType { get; internal set; }
    }
}
