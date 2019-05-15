using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesPodcastFinder.Models
{
    public interface IPodcastEpisode
    {
        int EpisodeNumber { get; }
        string Title { get; }
        string Editor { get; }
        string Summary { get; }
        DateTime PublishedDate { get; }
        Uri FileUrl { get; }
        TimeSpan Duration { get; }
    }
}
