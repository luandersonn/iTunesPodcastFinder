using System;

namespace iTunesPodcastFinder.Models
{
	public class Podcast
	{		
		public string Name { get; internal set; }
		public string Summary { get; internal set; }
		public string Editor { get; internal set; }
		public string ItunesLink { get; internal set; }
		public string FeedUrl { get; internal set; }
		public DateTime ReleaseDate { get; internal set; }
		public int EpisodesCount { get; internal set; }
		public string ArtWork { get; internal set; }		
		public string Genre { get; internal set; }
	}
}
