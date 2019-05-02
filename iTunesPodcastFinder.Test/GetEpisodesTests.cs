using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace iTunesPodcastFinder.Test
{
	public class GetEpisodesTests
	{
		PodcastFinder finder;
		[SetUp]
		public void Setup()
		{
			finder = new PodcastFinder();
		}

		[Test]
		public void GetEpisodesThrowsExpectedException()
		{
			string feedUrl = null;
			Assert.ThrowsAsync<ArgumentNullException>(() => finder.GetPodcastEpisodesAsync(feedUrl));

			feedUrl = "invalidURLteste";
			Assert.ThrowsAsync<UriFormatException>(() => finder.GetPodcastEpisodesAsync(feedUrl));

			feedUrl = "https://www.invalidURL.com/asdalsdkd";
			Assert.ThrowsAsync<HttpRequestException>(() => finder.GetPodcastEpisodesAsync(feedUrl));
		}

		[Test]
		public void ValidRss()
		{
			string feedUrl = @"https://anchor.fm/s/780305c/podcast/rss";
			var result = finder.GetPodcastEpisodesAsync(feedUrl).Result;
			foreach (var item in result)
			{
				Assert.IsNotNull(item.Title);
				Assert.IsNotNull(item.Editor);
				Assert.IsNotNull(item.FileUrl);
			}
		}
	}
}
