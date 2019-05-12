using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesPodcastFinder.Models
{
    public class PodcastRequestResult
    {
        internal PodcastRequestResult(Podcast podcast, IEnumerable<PodcastEpisode> episodes)
        {
            Podcast = podcast;
            Episodes = episodes;
        }

        public Podcast Podcast { get; }
        public IEnumerable<PodcastEpisode> Episodes { get; }
    }
}
