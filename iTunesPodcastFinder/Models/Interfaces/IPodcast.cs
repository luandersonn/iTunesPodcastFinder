using System;

namespace iTunesPodcastFinder.Models
{
    public interface IPodcast
    {
        string Name { get; }
        string Summary { get; }
        string Editor { get; }
		string Website { get; }
        string ItunesLink { get; }
        string ItunesId { get; }
        string FeedUrl { get; }
        DateTime ReleaseDate { get; }
        int EpisodesCount { get; }
        string ArtWork { get;  }
        string Genre { get; }
    }
}
