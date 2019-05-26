using System;

namespace iTunesPodcastFinder.Models
{
    /// <summary>
    /// It represents a podcast episode. Properties are self-explanatory.
    /// </summary>
	public class PodcastEpisode : IPodcastEpisode
    {
        public int EpisodeNumber { get; internal set; }
        public string Title { get; internal set; }
        public string Editor { get; internal set; }
        public string Summary { get; internal set; }
        public DateTime PublishedDate { get; internal set; }
        public Uri FileUrl { get; internal set; }
        public TimeSpan Duration { get; internal set; }
        public string InnerXml { get; internal set; }
    }
}
