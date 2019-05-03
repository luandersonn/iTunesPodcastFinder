using iTunesPodcastFinder.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iTunesPodcastFinder.Test
{
	public class SearchTests
	{
		PodcastFinder finder;
		[SetUp]
		public void Setup()
		{
			finder = new PodcastFinder();
		}

		[Test]
		public void ValidSearch()
		{
			int maxItems = 50;
			IEnumerable<Podcast> result = finder.SearchPodcastsAsync("The Verge", maxItems).Result;
			Assert.IsTrue(result.Any());
			Assert.IsTrue(result.Any(x => x.Editor == "The Verge"));
			Assert.LessOrEqual(result.Count(), maxItems);
			
			result = finder.SearchPodcastsAsync("Manual", maxItems).Result;
			Assert.IsTrue(result.Any());
			Assert.IsTrue(result.Any(x => x.Editor == "Manual do Usuário"));
			Assert.LessOrEqual(result.Count(), maxItems);
		}

		[Test]
		public void SearchThrowsExpectedException()
		{
			Assert.ThrowsAsync<ArgumentNullException>(() => finder.SearchPodcastsAsync(null));
			Assert.ThrowsAsync<ArgumentNullException>(() => finder.SearchPodcastsAsync("CBN", country: null));
			Assert.ThrowsAsync<ArgumentException>(() => finder.SearchPodcastsAsync("CBN", maxItems: 0));
			Assert.ThrowsAsync<ArgumentException>(() => finder.SearchPodcastsAsync("CBN", maxItems: -10));
		}
	}
}
