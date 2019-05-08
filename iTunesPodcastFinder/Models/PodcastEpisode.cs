using System;

namespace iTunesPodcastFinder.Models
{
	public class PodcastEpisode
	{
		public string Title { get; internal set; }
		public string Editor { get; internal set; }
		public string Summary { get; internal set; }		
		public DateTime PublishedDate { get; internal set; }
		public Uri FileUrl { get; internal set; }
		public double Duration { get; internal set; }		
	}
}
