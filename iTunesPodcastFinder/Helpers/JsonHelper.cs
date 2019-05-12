using iTunesPodcastFinder.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace iTunesPodcastFinder.Helpers
{
    internal static class JsonHelper
    {
        internal static IEnumerable<Podcast> DeserializePodcast(string json)
        {
            JObject root = JObject.Parse(json);
            if (root["results"] != null)
            {
                string entrys = root["results"].ToString();
                return DeserializePodcast1(JArray.Parse(entrys));
            }
            else
            {
                string entrys = root["feed"]["entry"].ToString();
                return DeserializePodcast2(JArray.Parse(entrys));
            }
        }

        private static IEnumerable<Podcast> DeserializePodcast1(JArray data)
        {
            foreach (JObject entry in data)
            {
                Podcast podcast = new Podcast();
                podcast.Editor = entry["artistName"]?.ToString();
                podcast.Name = entry["collectionName"]?.ToString();
                podcast.ItunesLink = entry["collectionViewUrl"]?.ToString();
                podcast.FeedUrl = entry["feedUrl"]?.ToString();
                _ = DateTime.TryParse(entry["releaseDate"] + "", out DateTime releaseDate);
                podcast.ReleaseDate = releaseDate;
                podcast.EpisodesCount = entry["trackCount"]?.ToObject<int>() ?? 0;
                podcast.Genre = entry["primaryGenreName"]?.ToString();
                podcast.ArtWork = entry["artworkUrl600"]?.ToString();
                podcast.Summary = null;
                yield return podcast;
            }
        }

        private static IEnumerable<Podcast> DeserializePodcast2(JArray data)
        {
            foreach (JObject entry in data)
            {
                Podcast podcast = new Podcast();
                podcast.Name = entry["im:name"]["label"].ToString();
                podcast.ArtWork = entry["im:image"][2]["label"].ToString();
                podcast.Summary = entry["summary"]?["label"]?.ToString();
                podcast.ItunesLink = entry["link"]["attributes"]["href"].ToString();
                podcast.Editor = entry["im:artist"]["label"].ToString();
                podcast.Genre = entry["category"]["attributes"]["label"].ToString();
                podcast.FeedUrl = null;
                DateTime? releaseDate = entry["im:releaseDate"]?["label"]?.ToObject<DateTime>();
                podcast.ReleaseDate = releaseDate.HasValue ? releaseDate.Value : default;
                yield return podcast;
            }
        }
    }
}
